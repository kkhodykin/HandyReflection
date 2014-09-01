using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HandyReflection.Core.Accessors;

namespace HandyReflection.Core
{

  class TypeReflector
  {
    private readonly IAccessorProvider _accessorProvider;

    public TypeReflector(IAccessorProvider accessorProvider)
    {
      _accessorProvider = accessorProvider;
    }


  }
}
