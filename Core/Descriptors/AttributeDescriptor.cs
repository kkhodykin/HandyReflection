using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandyReflection.Core.Descriptors
{
  public class AttributeDescriptor
  {
    public Type Type { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
  }
}