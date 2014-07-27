using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HandyReflection.Core.Descriptors;

namespace HandyReflection.Core.Accessors
{
  internal interface IPropertyAccessor : IMemberAccessor<PropertyDescriptor>
  {
    void Set(object value);
    object Get();
    TProperty Get<TProperty>();
  }

  class PropertyAccessor : MemberAccessorBase<PropertyDescriptor>, IPropertyAccessor
  {
    public void Set(object value)
    {
      throw new NotImplementedException();
    }

    public object Get()
    {
      throw new NotImplementedException();
    }

    public TProperty Get<TProperty>()
    {
      throw new NotImplementedException();
    }
  }
}