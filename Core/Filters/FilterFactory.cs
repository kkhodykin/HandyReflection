using System;
using System.Linq.Expressions;
using HandyReflection.Core.Descriptors;

namespace HandyReflection.Core.Filters
{
  internal class FilterFactory: ExpressionVisitor
  {
    public static MemberFilter CreateFilter<TDescriptor>(Expression expression) where TDescriptor : ReflectionDescriptorBase
    {
      var instance = new FilterFactory(typeof (TDescriptor));
      instance.Visit(expression);
      return instance._filter;
    }

    readonly MemberFilter _filter = new MemberFilter();

    protected FilterFactory(Type type)
    {
      
    }
  }
}