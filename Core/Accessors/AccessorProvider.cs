using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandyReflection.Core.Accessors
{
  public interface IAccessorProvider
  {
    TAccessor GetAccessor<TAccessor>();
  }

  internal class AccessorProvider : IAccessorProvider
  {
    public TAccessor GetAccessor<TAccessor>()
    {
      throw new NotImplementedException();
    }
  }
}
