using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HandyReflection.Core.Accessors;
using HandyReflection.Core.Descriptors;

namespace HandyReflection.Core.Extensions
{
  public static class PropertyExtensions
  {
    public static void Set(this IQueryable<PropertyDescriptor> source, object value)
    {
      source.GetAccessor<PropertyAccessor>().Set(value);
    }

    public static object Get(this IQueryable<PropertyDescriptor> source)
    {
      return source.GetAccessor<PropertyAccessor>().Get();
    }

    public static TProperty Get<TProperty>(this IQueryable<PropertyDescriptor> source)
    {
      return source.GetAccessor<PropertyAccessor>().Get<TProperty>();
    }
  }
}
