using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HandyReflection.Core.Accessors;
using HandyReflection.Core.Descriptors;

namespace HandyReflection.Core.Extensions
{
  public static class QueryableExtensions
  {
    public static void Set(this IQueryable<PropertyDescriptor> source, object value)
    {
      var propertyAccessor = source as IPropertyAccessor;
      if (propertyAccessor == null)
        throw new ArgumentException("source should be PropertyAccessor");
      
      propertyAccessor.Set(value);
    }

    public static object Get(this IQueryable<PropertyDescriptor> source)
    {
      var propertyAccessor = source as IPropertyAccessor;
      if (propertyAccessor == null)
        throw new ArgumentException("source should be PropertyAccessor");

      return propertyAccessor.Get();
    }

    public static TProperty Get<TProperty>(this IQueryable<PropertyDescriptor> source)
    {
      var propertyAccessor = source as IPropertyAccessor;
      if (propertyAccessor == null)
        throw new ArgumentException("source should be PropertyAccessor");

      return propertyAccessor.Get<TProperty>();
    }
  }
}
