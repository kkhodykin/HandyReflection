using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using HandyReflection.Core.Descriptors;

namespace HandyReflection.Core.Filters
{
  enum FilterGroupKind
  {
    Root = 0, And = 1, Or = 2
  }

  enum FilterExpression
  {
    Unknown = 0, Eq = 13, Ne = 35
  }

  enum FilterBy
  {
    NotSet = 0, BindingFlags = 1, Name = 3, Type = 4, MemberType = 5, Unknown = 99
  }

  enum FilterType
  {
    Group, Filter
  }

  internal interface IFilter
  {
    FilterType FilterType { get; }
  }

  internal class FilterGroup : IFilter
  {
    public Stack<IFilter> Filters { get; private set; }
    public FilterGroupKind GroupKind { get; set; }

    public FilterGroup()
    {
      Filters = new Stack<IFilter>();
    }

    public FilterType FilterType { get { return FilterType.Group; } }
  }

  class Filter : IFilter
  {
    public FilterBy FilterBy { get; set; }
    public FilterExpression FilterExpression { get; set; }

    public string Name { get; set; }
    public BindingFlags BindingFlags { get; set; }
    public MemberTypes MemberTypes { get; set; }
    public Type Type { get; set; }

    public FilterType FilterType { get { return FilterType.Filter; } }
  }



  class MemberFilterAggregator
  {
    private readonly FilterGroup _filterRoot = new FilterGroup { GroupKind = FilterGroupKind.Root };

    private static readonly Dictionary<Type, FilterBy> PropertyTypes = new Dictionary<Type, FilterBy>
    {
      {typeof (MemberAccessMode), FilterBy.BindingFlags},
      {typeof (MemberVisibility), FilterBy.BindingFlags},
      {typeof(MemberTypes), FilterBy.MemberType}
    };

    private static readonly BindingFlags[] BindingFlagsMappinsg =
    {
      BindingFlags.Public, BindingFlags.NonPublic,
      BindingFlags.Instance, BindingFlags.Static
    };

    public virtual FilterGroup Push(Expression item, FilterGroup root = null)
    {
      root = root ?? _filterRoot;
      switch (item.NodeType)
      {
        case ExpressionType.Or:
        case ExpressionType.OrElse:
          return ProcessOr(root);
        case ExpressionType.NotEqual:
        case ExpressionType.Equal:
          ProcessCondition((FilterExpression)(int)item.NodeType, root);
          break;
        case ExpressionType.MemberAccess:
          ProcessMember(item as MemberExpression, root);
          break;
        case ExpressionType.Constant:
          ProcessValue(item as ConstantExpression, root);
          break;
      }
      return root;
    }

    public MemberCacheDescriptor ToMemberCacheDescriptor()
    {
      if (!_filterRoot.Filters.Any()) return null;

      return ParseFilterTree(_filterRoot);
    }

    private MemberCacheDescriptor ParseFilterTree(FilterGroup root)
    {
      return ToDescriptor(root);
    }

    private MemberCacheDescriptor ToDescriptor(FilterGroup group)
    {
      var descriptors = new List<MemberCacheDescriptor>();

      while (group.Filters.Any())
      {
        var filter = group.Filters.Pop();
        switch (filter.FilterType)
        {
          case FilterType.Group:
            descriptors.Add(ToDescriptor((FilterGroup)filter));
            break;
          case FilterType.Filter:
            descriptors.Add(ToDescriptor((Filter)filter));
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }

      switch (group.GroupKind)
      {
        case FilterGroupKind.And:
        case FilterGroupKind.Root:
          return MergeDescriptorsUsingAnd(descriptors);
        case FilterGroupKind.Or:
          return MergeDescriptorsUsingOr(descriptors);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    private MemberCacheDescriptor ToDescriptor(Filter filter)
    {
      var result = new MemberCacheDescriptor();
      switch (filter.FilterBy)
      {
        case FilterBy.BindingFlags:
          result.BindingFlags = filter.BindingFlags;
          break;
        case FilterBy.Name:
          result.Name = filter.Name;
          break;
        case FilterBy.Type:
          result.Type = filter.Type;
          break;
        case FilterBy.MemberType:
          result.MemberTypes = filter.MemberTypes;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }

      return result;
    }

    private MemberCacheDescriptor MergeDescriptorsUsingAnd(IEnumerable<MemberCacheDescriptor> descriptors)
    {
      var result = new MemberCacheDescriptor();
      foreach (var descriptor in descriptors)
      {
        result.BindingFlags = MergeFlags(result.BindingFlags, descriptor.BindingFlags);
        if (descriptor.MemberTypes != MemberTypes.All && result.MemberTypes == MemberTypes.All)
          result.MemberTypes = descriptor.MemberTypes;
        else if (descriptor.MemberTypes != MemberTypes.All)
          result.MemberTypes = ~MemberTypes.All;
        if (!string.IsNullOrEmpty(descriptor.Name) && string.IsNullOrEmpty(result.Name))
          result.Name = descriptor.Name;
        else if (!string.IsNullOrEmpty(descriptor.Name))
          throw new ArgumentException("Mutually exclusive conditions for Name");
        if (descriptor.Type !=null && result.Type == null)
          result.Type = descriptor.Type;
        else if (descriptor.Type != null)
          throw new ArgumentException("Mutually exclusive conditions for Type");
      }
      return result;
    }

    private MemberCacheDescriptor MergeDescriptorsUsingOr(IEnumerable<MemberCacheDescriptor> descriptors)
    {
      var result = new MemberCacheDescriptor();
      foreach (var descriptor in descriptors)
      {
        result.BindingFlags |= descriptor.BindingFlags;
        result.MemberTypes |= descriptor.MemberTypes;
        if (result.Type == null && descriptor.Type != null)
          result.Type = descriptor.Type;
        else if(descriptor.Type!=null)
          throw new ArgumentException("Can't filter by multiple types");
        if (!string.IsNullOrEmpty(descriptor.Name) && string.IsNullOrEmpty(result.Name))
          result.Name = descriptor.Name;
        else if (!string.IsNullOrEmpty(descriptor.Name))
          throw new ArgumentException("Can't filter by multiple names");
      }

      return result;
    }

    private BindingFlags MergeFlags(BindingFlags a, BindingFlags b)
    {
      switch (b)
      {
        case BindingFlags.Public:
          if ((a & BindingFlags.NonPublic) == BindingFlags.NonPublic)
            return a & ~BindingFlags.NonPublic;
          break;
        case BindingFlags.NonPublic:
          if ((a & BindingFlags.Public) == BindingFlags.Public)
            return a & ~BindingFlags.Public;
          break;
        case BindingFlags.Instance:
          if ((a & BindingFlags.Static) == BindingFlags.Static)
            return a & ~BindingFlags.Static;
          break;
        case BindingFlags.Static:
          if ((a & BindingFlags.Instance) == BindingFlags.Instance)
            return a & ~BindingFlags.Instance;
          break;
      }
      return a | b;
    }

    private FilterGroup ProcessOr(FilterGroup root)
    {
      var group = new FilterGroup {GroupKind = FilterGroupKind.Or};
      root.Filters.Push(group);
      return group;
    }

    private void ProcessMember(MemberExpression expression, FilterGroup root)
    {
      var filterBy = ResolvePropertyType(expression.Member);
      if (filterBy == FilterBy.Unknown && root.Filters.Peek() is Filter)
        root.Filters.Pop();

      var filter = (Filter)root.Filters.Peek();
      filter.FilterBy = filterBy;
    }

    private void ProcessValue(ConstantExpression item, FilterGroup root)
    {
      if (root == null)
        throw new ArgumentException("the filter should be of specific type");
      var filter = root.Filters.Peek() as Filter;
      if (filter == null)
        return;

      switch (filter.FilterBy)
      {
        case FilterBy.NotSet:
          root.Filters.Pop();
          break;
        case FilterBy.BindingFlags:
          var intValue = (int)item.Value;
          filter.BindingFlags = BindingFlagsMappinsg[intValue - 1];
          break;
        case FilterBy.Name:
          filter.Name = item.Value.ToString();
          break;
        case FilterBy.Type:
          filter.Type = item.Value as Type;
          break;
        case FilterBy.MemberType:
          filter.MemberTypes = (MemberTypes)item.Value;
          break;
        default:
          root.Filters.Pop();
          break;
      }
    }

    private void ProcessCondition(FilterExpression filterExpression, FilterGroup root)
    {
      var filter = new Filter();
      root.Filters.Push(filter);
      filter.FilterExpression = filterExpression;
    }

    private FilterBy ResolvePropertyType(MemberInfo info)
    {
      var property = info as PropertyInfo;
      if (property == null)
        return FilterBy.Unknown;
      if (property.PropertyType == typeof(string) && property.Name == "Name" && typeof(MemberDescriptorBase).IsAssignableFrom(property.DeclaringType))
        return FilterBy.Name;
      if (property.PropertyType == typeof(Type) && property.Name == "Type" && typeof(MemberDescriptorBase).IsAssignableFrom(property.DeclaringType))
        return FilterBy.Type;
      if (PropertyTypes.ContainsKey(property.PropertyType))
        return PropertyTypes[property.PropertyType];
      return FilterBy.Unknown;
    }
  }
}
