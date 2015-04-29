using System;
using System.Linq;
using System.Reflection;
using HandyReflection.Core.Descriptors;
using HandyReflection.Core.Linq;
using Leverate.Reflection;

namespace HandyReflection.Core.Accessors
{
  internal interface IPropertyAccessor : IMemberAccessor<PropertyDescriptor>
  {
    void Set(object value);
    object Get();
    TProperty Get<TProperty>();
  }

  class PropertyAccessor : MemberAccessor<PropertyDescriptor>, IPropertyAccessor
  {
    public void Set(object value)
    {
      throw new NotImplementedException();
    }

    public object Get()
    {
      //var model = QueryParser.CreateDefault().GetParsedQuery(Expression);
      //var cacheDescriptor = MemberBaseVisitor.Visit(model);
      //cacheDescriptor.MemberTypes = MemberTypes.Property;
      //var property = MemberCache.Default.Get<PropertyInfo>(cacheDescriptor).FirstOrDefault();
      //if(property == null)
      //  throw new InvalidOperationException("Property not found");
      //return property.GetGetter<object, object>()(Instance);
      throw new NotImplementedException();
    }

    public TProperty Get<TProperty>()
    {
      throw new NotImplementedException();
    }
  }
}