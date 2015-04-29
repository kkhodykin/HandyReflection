using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HandyReflection.Core.Descriptors;

namespace HandyReflection.Core
{
  public class MemberCacheDescriptor
  {
		public const BindingFlags DefaultFlags = BindingFlags.Instance | BindingFlags.Public;
	  public const BindingFlags StaticFlags = BindingFlags.Static | BindingFlags.Public;
	  public const BindingFlags NonPublicFlags = BindingFlags.Instance | BindingFlags.NonPublic;
	  public const BindingFlags StaticNonPublicFlags = BindingFlags.Static | BindingFlags.NonPublic;
	  public const BindingFlags AnyMemberFlags =
		  BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    public Type Type { get; set; }
    public string Name { get; set; }
    public MemberTypes MemberTypes { get; set; }
    public BindingFlags BindingFlags { get; set; }

    public MemberCacheDescriptor()
    {
      MemberTypes = MemberTypes.All;
	    BindingFlags = DefaultFlags;
    }

    public MemberCacheDescriptor(MemberDescriptorBase descriptor)
    {
	    MemberTypes = descriptor.MemberTypes;
	    BindingFlags = GetFlags(descriptor.AccessMode) | GetFlags(descriptor.Visibility);
	    Type = descriptor.Type;
	    Name = descriptor.Name;
    }

	  private BindingFlags GetFlags(MemberAccessMode accessMode)
	  {
		  switch (accessMode)
		  {
			  case MemberAccessMode.Any:
				  return BindingFlags.Instance | BindingFlags.Static;
			  case MemberAccessMode.Instance:
				  return BindingFlags.Instance;
			  case MemberAccessMode.Static:
				  return BindingFlags.Static;
		  }
		  return DefaultFlags;
	  }

	  private BindingFlags GetFlags(MemberVisibility accessMode)
	  {
		  switch (accessMode)
		  {
			  case MemberVisibility.Any:
				  return BindingFlags.Public | BindingFlags.NonPublic;
			  case MemberVisibility.Public:
				  return BindingFlags.Public;
			  case MemberVisibility.NonPublic:
				  return BindingFlags.NonPublic;
		  }

		  return DefaultFlags;
	  }

    public MemberCacheDescriptor(Type type, string name)
      : this()
    {
      Type = type;
      Name = name;
    }
  }

  internal class MemberCache
  {
    private static readonly Dictionary<Type, MemberTypes> TypeMemberTypeMap = new Dictionary<Type, MemberTypes>
        {
          {typeof(ConstructorInfo), MemberTypes.Constructor},
          {typeof(PropertyInfo), MemberTypes.Property},
          {typeof(MethodInfo), MemberTypes.Method},
          {typeof(FieldInfo), MemberTypes.Field},
        };

    private static readonly ConcurrentDictionary<string, IEnumerable<MemberInfo>> Cache = new ConcurrentDictionary<string, IEnumerable<MemberInfo>>();

    private static readonly Lazy<MemberCache> DefaultInstance = new Lazy<MemberCache>(() => new MemberCache());

    public static MemberCache Default
    {
      get { return DefaultInstance.Value; }
    }

    public IEnumerable<MemberInfo> Get(MemberCacheDescriptor cacheDescriptor)
    {
      return SplitCacheRequest(cacheDescriptor).SelectMany(x => Cache.GetOrAdd(x.Key, x.Value));
    }

    public IEnumerable<TMember> Get<TMember>(MemberCacheDescriptor cacheDescriptor)
      where TMember : MemberInfo
    {
      return (Get(cacheDescriptor) ?? Enumerable.Empty<TMember>()).OfType<TMember>();
    }


    public IEnumerable<TMember> Get<TMember>(Type type, string name)
      where TMember : MemberInfo
    {
      return
        Get<TMember>(new MemberCacheDescriptor(type, name)
        {
          MemberTypes =
            TypeMemberTypeMap.ContainsKey(typeof(TMember)) ? TypeMemberTypeMap[typeof(TMember)] : MemberTypes.All
        });
    }

    private static IEnumerable<KeyValuePair<string, Func<string, IEnumerable<MemberInfo>>>> SplitCacheRequest(MemberCacheDescriptor cacheDescriptor)
    {
      var requestedTypes = SplitMemberTypes(cacheDescriptor.MemberTypes);
      var requestedFlags = SplitFlags(cacheDescriptor.BindingFlags);

      var result = requestedTypes.SelectMany(
        t => requestedFlags.Select(f => new
        {
          Key = string.Format("{0}${1}${2}${3}", cacheDescriptor.Type.FullName, cacheDescriptor.Name, t, f),
          Value = MakeMemberFactory(cacheDescriptor.Type, cacheDescriptor.Name, t, f)
        }));

      return result.ToDictionary(x => x.Key, x => x.Value);
    }

    private static Func<string, IEnumerable<MemberInfo>> MakeMemberFactory(Type type, string memberName, MemberTypes memberTypes,
      BindingFlags flags)
    {
      return v => type.GetMember(memberName, memberTypes, flags);
    }

    private static IEnumerable<MemberTypes> SplitMemberTypes(MemberTypes types)
    {
      Func<MemberTypes, MemberTypes, MemberTypes?> func =
        (v, t) => ((v & t) == t) ? t : (MemberTypes?)null;

      return new[]
      {
        MemberTypes.Constructor, MemberTypes.Event, MemberTypes.Field, MemberTypes.Method, MemberTypes.Property,
        MemberTypes.TypeInfo, MemberTypes.Custom, MemberTypes.NestedType,
      }.Select(x => func(types, x)).Where(x => x.HasValue).Select(x => x.Value); ;
    }

    //public_static, non-public_static public_instance, non-public_instance
    private static IEnumerable<BindingFlags> SplitFlags(BindingFlags flags)
    {
      if (flags == BindingFlags.Default)
        return new[] { BindingFlags.Public | BindingFlags.Instance };

      Func<BindingFlags, BindingFlags, BindingFlags?> func =
        (v, t) => ((v & t) == t) ? t : (BindingFlags?)null;

      return new[]
      {
        BindingFlags.Instance | BindingFlags.Public,
        BindingFlags.Instance | BindingFlags.NonPublic,
        BindingFlags.Static | BindingFlags.Public,
        BindingFlags.Static | BindingFlags.NonPublic
      }
        .Select(x => func(flags, x)).Where(x => x.HasValue).Select(x => x.Value);
    }
  }
}