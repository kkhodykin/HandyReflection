using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HandyReflection.Core.Accessors;

namespace HandyReflection.Core.Extensions
{
  public static class ObjectExtensions
  {
    public static IMemberAccessBuilder Reflect(this object target)
    {
      var accessorProvider =
        (IAccessorProvider) GlobalConfiguration.DependencyResolver.GetService(typeof (IAccessorProvider));

      if (target is Type)
        return new MemberAccessBuilder(accessorProvider);
      return new MemberAccessBuilder(accessorProvider, target);
    }

    internal static TAccessor GetAccessor<TAccessor>(this object source)
    {
      if (!(source is TAccessor))
        throw new ArgumentException("Invalid type", "source");

      return (TAccessor)source;
    }
  }
}
