using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HandyReflection.Core.Descriptors
{
  public class PropertyDescriptor : MemberDescriptorBase
  {
    public PropertyInfo PropertyInfo { get; set; }
  }
}