using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using HandyReflection.Core.Descriptors;

namespace HandyReflection.Core.Filters
{
  class FilterScopeStack : ConcurrentStack<Filter>
  {
    public Filter GetScope()
    {
      Filter result;
      if (TryPeek(out result) && !result.Cancelled)
        return result;
      result = new Filter();
      Push(result);
      return result;
    }
  }

  enum FilterGroupping
  {
    Empty = 0, And = 1, Or = 2
  }

  enum FilterFunction
  {
    Empty = 0, Eq = 13, Ne = 35
  }

  enum FilterBy
  {
    Empty = 0, BindingFlags = 1, Name = 3, Type = 4, MemberType = 5
  }

  internal class CompositeFilter
  {
    public IList<CompositeFilter> ChildScopes { get; private set; }
    public bool Cancelled { get; set; }
    public FilterGroupping FilterGroupping { get; set; }
    public CompositeFilter()
    {
      ChildScopes = new List<CompositeFilter>();
    }
  }

  class Filter : CompositeFilter
  {
    private FilterBy _filterBy;

    public FilterBy FilterBy
    {
      get { return _filterBy; }
      set
      {
        _filterBy = value;
        UpdateState();
      }
    }

    public BindingFlags BindingFlags { get; set; }
    public MemberTypes MemberTypes { get; set; }
    public Type Type { get; set; }

    public FilterFunction FilterFunction { get; set; }



    private void UpdateState()
    {
      
    }
  }

  class MemberFilterAggregator
  {
    private readonly CompositeFilter _filterRoot = new CompositeFilter { FilterGroupping = FilterGroupping.Empty};
    private static readonly Dictionary<Type, FilterBy> PropertyTypes = new Dictionary<Type, FilterBy>(new Dictionary<Type, FilterBy>
    {
      {typeof(MemberAccessMode), FilterBy.BindingFlags},
      {typeof(MemberVisibility), FilterBy.BindingFlags},
      {typeof(Type), FilterBy.Type},
    });

    public virtual CompositeFilter Push(Expression item, CompositeFilter root = null)
    {
      switch (item.NodeType)
      {
        case ExpressionType.And:
        case ExpressionType.AndAlso:
          return ProcessLogicalAnd(root);
          break;
        case ExpressionType.Or:
        case ExpressionType.OrElse:
          break;
        case ExpressionType.NotEqual:
        case ExpressionType.Equal:
          return ProcessCondition((FilterFunction) (int) item.NodeType,  root);
          break;
        case ExpressionType.MemberAccess:
          //_itemScopes.GetScope().FilterBy = ResolvePropertyType((item as MemberExpression).Member);
          break;
        case ExpressionType.Constant:
          return ProcessValue(item as ConstantExpression, root as Filter);
          break;
      }
    }

    private CompositeFilter ProcessValue(ConstantExpression item, Filter root)
    {
      if(root == null)
        throw new ArgumentException("the filter should be of specific type");

      switch (root.FilterBy)
      {
        case FilterBy.Empty:
          root.Cancelled = true;
          break;
        case FilterBy.BindingFlags:

          break;
        case FilterBy.Name:
          break;
        case FilterBy.Type:
          break;
        case FilterBy.MemberType:
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      root.FilterFunction
    }

    private CompositeFilter ProcessCondition(FilterFunction filterFunction, CompositeFilter root)
    {
      root.ChildScopes.Add(new Filter{FilterGroupping = FilterGroupping.Or, FilterFunction = filterFunction});
      return root.ChildScopes.Last();
    }

    private CompositeFilter ProcessLogicalAnd(CompositeFilter compositeFilter)
    {
      var currentFilter = compositeFilter ?? _filterRoot;
      if (currentFilter.FilterGroupping == FilterGroupping.Empty || currentFilter.FilterGroupping == FilterGroupping.And)
      {
        currentFilter.FilterGroupping = FilterGroupping.And;
        return currentFilter;
      }

      var filter = new CompositeFilter {FilterGroupping = FilterGroupping.And};
      currentFilter.ChildScopes.Add(filter);
      return filter;
    }



    private FilterBy ResolvePropertyType(MemberInfo info)
    {
      var property = info as PropertyInfo;
      if(property == null || !PropertyTypes.ContainsKey(property.PropertyType))
        return FilterBy.Empty;
      return PropertyTypes[property.PropertyType];
    }



  }
}
