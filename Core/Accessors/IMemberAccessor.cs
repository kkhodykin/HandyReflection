using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HandyReflection.Core.Descriptors;

namespace HandyReflection.Core.Accessors
{
  public interface IAccessor
  {
    IAccessor SetInstance(object instance);
  }

	public interface IMemberAccessor<TDescriptor> : IAccessor, IQueryable<TDescriptor>
//    where TDescriptor : ReflectionDescriptorBase
  {
    TAccessor SetInstance<TAccessor>(object instance) where TAccessor : IMemberAccessor<TDescriptor>;
    bool HasInstance();
  }
}