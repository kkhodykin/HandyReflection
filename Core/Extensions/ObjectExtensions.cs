using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandyReflection.Core.Extensions
{
  public static class ObjectExtensions
  {
    public static IMemberAccessBuilder Reflect(this object target)
    {
      if (target is Type)
        return new MemberAccessBuilder();
      return new MemberAccessBuilder(target);
    }
  }
}
