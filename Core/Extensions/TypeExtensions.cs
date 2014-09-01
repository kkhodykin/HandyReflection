using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HandyReflection.Core.Accessors;

namespace HandyReflection.Core.Extensions
{
  public static class TypeExtensions
  {
    public static IConsructorAccessor ReflectType(this Type type)
    {
      return new ConstructorAccessor(type);
    }
  }
}
