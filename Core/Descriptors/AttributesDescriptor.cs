using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace HandyReflection.Core.Descriptors
{
  public interface IAttributesDescriptor : IEnumerable<Attribute>
  {
    bool Inherit { get; }
  }

  class AttributesDescriptor : List<Attribute>, IAttributesDescriptor
  {
    public bool Inherit { get; set; }
  }
}
