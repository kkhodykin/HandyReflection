using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using HandyReflection.Core.Descriptors;
using HandyReflection.Core.Filters;
using HandyReflection.Core.Linq;
using Remotion.Linq;
using Remotion.Linq.Parsing.Structure;

namespace HandyReflection.Core.Accessors
{

  class MemberAccessor<TDescriptor> : QueryableBase<TDescriptor> //:IQueryProvider, IMemberAccessor<TDescriptor>
  {
    public object Instance { get; private set; }

    public MemberAccessor()
      : this(QueryParser.CreateDefault(), new MemberQueryExecutor())
    {

    }

    protected MemberAccessor(IQueryParser queryParser, IQueryExecutor executor)
      : base(queryParser, executor)
    {
    }

    protected MemberAccessor(IQueryProvider provider)
      : base(provider)
    {
    }

    public MemberAccessor(IQueryProvider provider, Expression expression)
      : base(provider, expression)
    {
    }

    public TAccessor SetInstance<TAccessor>(object instance) where TAccessor : IMemberAccessor<TDescriptor>
    {
      Instance = instance;
      return (TAccessor)(IMemberAccessor<TDescriptor>)this;
    }

    public IAccessor SetInstance(object instance)
    {
      throw new NotImplementedException();
    }

    public bool HasInstance()
    {
      return Instance != null;
    }
  }
}