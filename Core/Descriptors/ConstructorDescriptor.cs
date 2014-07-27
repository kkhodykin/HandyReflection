using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HandyReflection.Core.Descriptors
{
  public class ConstructorDescriptor : ReflectionDescriptorBase
  {
    public ConstructorInfo ConstructorInfo { get; internal set; }
  }
}
