using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HandyReflection.Core.Descriptors
{
  public class MethodDescriptor : MemberDescriptorBase
  {
    public MethodInfo MethodInfo { get; internal set; }
  }
}