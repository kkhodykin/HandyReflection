using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HandyReflection.Core.DependencyInjection;

namespace HandyReflection.Core
{
  public static class GlobalConfiguration
  {
    static readonly Lazy<IDependencyResolver> DefaultDependencyResolver = new Lazy<IDependencyResolver>(()=>new DefaultDependencyResolver());

    private static IDependencyResolver _dependencyResolver;

    public static IDependencyResolver DependencyResolver
    {
      get { return _dependencyResolver ?? DefaultDependencyResolver.Value; }
      set { _dependencyResolver = value; }
    }
  }
}