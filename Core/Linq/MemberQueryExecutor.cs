using System;
using System.Collections.Generic;
using System.Linq;
using Remotion.Linq;

namespace HandyReflection.Core.Linq
{
  class MemberQueryExecutor : IQueryExecutor
  {
    public T ExecuteScalar<T>(QueryModel queryModel)
    {
      return ExecuteCollection<T>(queryModel).Single();
    }

    public T ExecuteSingle<T>(QueryModel queryModel, bool returnDefaultWhenEmpty)
    {
      return returnDefaultWhenEmpty ? ExecuteCollection<T>(queryModel).SingleOrDefault() : ExecuteCollection<T>(queryModel).Single();
    }

    public IEnumerable<T> ExecuteCollection<T>(QueryModel queryModel)
    {
      new MemberBaseVisitor().VisitQueryModel(queryModel);
      return Enumerable.Empty<T>();
    }
  }
}