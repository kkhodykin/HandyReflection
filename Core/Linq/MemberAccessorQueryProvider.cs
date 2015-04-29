using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using HandyReflection.Core.Accessors;

namespace HandyReflection.Core.Linq
{
  class MemberAccessorQueryProvider : IQueryProvider
  {
    public IQueryable CreateQuery(Expression expression)
    {
      throw new NotImplementedException();
    }

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
    {
      return new MemberAccessor<TElement>(this, expression);
    }

    public object Execute(Expression expression)
    {
      
      throw new NotImplementedException();
    }

    public TResult Execute<TResult>(Expression expression)
    {
      var descriptor = MemberBaseVisitor.GetCacheDescriptor(expression);
      MemberCache.Default.Get(descriptor);
      return default(TResult);
    }
  }
}
