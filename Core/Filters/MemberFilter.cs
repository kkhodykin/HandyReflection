using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace HandyReflection.Core.Filters
{
  class MemberFilter : ExpressionVisitor
  {
    public BindingFlags BindingFlags { get; private set; }
    public String Name { get; private set; }
    public IEnumerable<Type> AtributeTypes { get; private set; }

    public MemberFilter()
    {
      BindingFlags = BindingFlags.Instance | BindingFlags.Public;
    }
  }
}
