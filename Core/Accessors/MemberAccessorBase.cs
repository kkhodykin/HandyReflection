using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using HandyReflection.Core.Descriptors;
using HandyReflection.Core.Filters;

namespace HandyReflection.Core.Accessors
{
  abstract class MemberAccessorBase<TDescriptor> :IQueryProvider, IMemberAccessor<TDescriptor> where TDescriptor : ReflectionDescriptorBase
  {
    private IQueryable<TDescriptor> _queryable = new List<TDescriptor>().AsQueryable();
    public object Instance { get; private set; }

    protected MemberAccessorBase(object instance)
    {
      Instance = instance;
    }

    public TAccessor SetInstance<TAccessor>(object instance)where TAccessor : IMemberAccessor<TDescriptor>
    {
      Instance = instance;
      return (TAccessor)(IMemberAccessor<TDescriptor>)this;
    }

    public bool HasInstance()
    {
      throw new NotImplementedException();
    }

    public IEnumerator<TDescriptor> GetEnumerator()
    {
      throw new NotImplementedException();
    }

    protected MemberFilter GetFilter()
    {
      return FilterFactory.CreateFilter<TDescriptor>(Expression);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public Expression Expression { get { return _queryable.Expression; } }
    public Type ElementType { get { return _queryable.ElementType; } }
    public IQueryProvider Provider { get { return _queryable.Provider; } }
    public IQueryable CreateQuery(Expression expression)
    {
      _queryable = (IQueryable<TDescriptor>) _queryable.Provider.CreateQuery(expression);
      return this;
    }

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
    {
      _queryable = (IQueryable<TDescriptor>) _queryable.Provider.CreateQuery<TElement>(expression);
      return (IQueryable<TElement>) this;
    }

    public object Execute(Expression expression)
    {
      throw new NotImplementedException();
    }

    public TResult Execute<TResult>(Expression expression)
    {
      throw new NotImplementedException();
    }
  }
}