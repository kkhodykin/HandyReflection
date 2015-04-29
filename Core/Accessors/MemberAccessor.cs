using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HandyReflection.Core.Linq;

namespace HandyReflection.Core.Accessors
{

  class MemberAccessor<TDescriptor> : IMemberAccessor<TDescriptor>
  {
    public object Instance { get; private set; }

    public MemberAccessor()
    {
      Provider = new MemberAccessorQueryProvider();
      Expression = Expression.Constant(this);
    }

    public MemberAccessor(MemberAccessorQueryProvider provider, Expression expression)
    {
      if (provider == null)
      {
        throw new ArgumentNullException("provider");
      }

      if (expression == null)
      {
        throw new ArgumentNullException("expression");
      }

      if (!typeof(IQueryable<TDescriptor>).IsAssignableFrom(expression.Type))
      {
        throw new ArgumentOutOfRangeException("expression");
      }

      Provider = provider;
      Expression = expression;
    }

    public TAccessor SetInstance<TAccessor>(object instance) where TAccessor : IMemberAccessor<TDescriptor>
    {
      Instance = instance;
      return (TAccessor)(IMemberAccessor<TDescriptor>)this;
    }

    public IAccessor SetInstance(object instance)
    {
      Instance = instance;
      return this;
    }

    public bool HasInstance()
    {
      return Instance != null;
    }

    public IEnumerator<TDescriptor> GetEnumerator()
    {
      return (Provider.Execute<IEnumerable<TDescriptor>>(Expression)).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (Provider.Execute<System.Collections.IEnumerable>(Expression)).GetEnumerator();
    }

    public Expression Expression { get; private set; }
    public IQueryProvider Provider { get; private set; }

    public Type ElementType
    {
      get { return typeof (TDescriptor); }
    }

  }
}