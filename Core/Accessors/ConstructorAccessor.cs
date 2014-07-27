using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HandyReflection.Core.Descriptors;

namespace HandyReflection.Core.Accessors
{
  internal interface IConsructorAccessor : IMemberAccessor<ConstructorDescriptor>
  {
    TInstance Instantiate<TInstance>(params object[] parameters);
  }

  class ConstructorAccessor : MemberAccessorBase<ConstructorDescriptor>, IConsructorAccessor
  {
    public TInstance Instantiate<TInstance>(params object[] parameters)
    {
      throw new NotImplementedException();
    }
  }
}