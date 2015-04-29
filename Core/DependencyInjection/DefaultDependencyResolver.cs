using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HandyReflection.Core.Accessors;

namespace HandyReflection.Core.DependencyInjection
{
  public interface IDependencyResolver
  {
    object GetService(Type serviceType);
    IEnumerable<object> GetServices(Type serviceType);
  }

  class DefaultDependencyResolver : IDependencyResolver
  {
    public object GetService(Type serviceType)
    {
      if (serviceType == typeof (IAccessorProvider))
        return new AccessorProvider();

      throw new NotImplementedException();
    }

    public IEnumerable<object> GetServices(Type serviceType)
    {
      throw new NotImplementedException();
    }
  }
}
