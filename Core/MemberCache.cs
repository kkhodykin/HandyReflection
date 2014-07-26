using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Leverate.Reflection
{
  class MemberDescriptor
  {
    public Type Type { get; set; }
    public string Name { get; set; }
    public MemberTypes MemberTypes { get; set; }
    public BindingFlags BindingFlags { get; set; }

    public MemberDescriptor()
    {
      MemberTypes = MemberTypes.All;
      BindingFlags = BindingFlags.Default;
    }

    public MemberDescriptor(Type type, string name)
      : this()
    {
      Type = type;
      Name = name;
    }
  }

  static class MemberCache
  {
    private static readonly Dictionary<Type, MemberTypes> TypeMemberTypeMap = new Dictionary<Type, MemberTypes>
    {
      {typeof(ConstructorInfo), MemberTypes.Constructor},
      {typeof(PropertyInfo), MemberTypes.Property},
      {typeof(MethodInfo), MemberTypes.Method},
      {typeof(FieldInfo), MemberTypes.Field},
    };

    private static readonly ConcurrentDictionary<string, IEnumerable<MemberInfo>> Cache = new ConcurrentDictionary<string, IEnumerable<MemberInfo>>();

    public static IEnumerable<MemberInfo> Get(MemberDescriptor descriptor)
    {
      return SplitCacheRequest(descriptor).SelectMany(x => Cache.GetOrAdd(x.Key, x.Value));
    }

    public static IEnumerable<TMember> Get<TMember>(MemberDescriptor descriptor)
      where TMember : MemberInfo
    {
      return (Get(descriptor) ?? Enumerable.Empty<TMember>()).OfType<TMember>();
    }


    public static IEnumerable<TMember> Get<TMember>(Type type, string name)
      where TMember : MemberInfo
    {
      return
        Get<TMember>(new MemberDescriptor(type, name)
        {
          MemberTypes =
            TypeMemberTypeMap.ContainsKey(typeof (TMember)) ? TypeMemberTypeMap[typeof (TMember)] : MemberTypes.All
        });
    }

    private static IEnumerable<KeyValuePair<string, Func<string, IEnumerable<MemberInfo>>>> SplitCacheRequest(MemberDescriptor descriptor)
    {
      var requestedTypes = SplitMemberTypes(descriptor.MemberTypes);
      var requestedFlags = SplitFlags(descriptor.BindingFlags);

      var result = requestedTypes.SelectMany(
        t => requestedFlags.Select(f => new
        {
          Key = string.Format("{0}${1}${2}${3}", descriptor.Type.FullName, descriptor.Name, t, f),
          Value = (Func<string, IEnumerable<MemberInfo>>)(v => descriptor.Type.GetMember(descriptor.Name, t, f))
        }));

      return result.ToDictionary(x => x.Key, x => x.Value);
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