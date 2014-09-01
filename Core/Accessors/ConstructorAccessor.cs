using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HandyReflection.Core.Descriptors;
using HandyReflection.Core.Helpers;

namespace HandyReflection.Core.Accessors
{
  public interface IConsructorAccessor// : IMemberAccessor<ConstructorDescriptor>
  {
    TInstance Instantiate<TInstance>(params object[] parameters);
    object Instantiate(params object[] parameters);
  }

  class ConstructorAccessor : IConsructorAccessor// : MemberAccessorBase<ConstructorDescriptor>, IConsructorAccessor
  {
    private readonly Type _type;

    public ConstructorAccessor(Type type)
    {
      _type = type;
    }

    public TInstance Instantiate<TInstance>(params object[] parameters)
    {
      return (TInstance)TypeHelper.GetConstructorCall(_type, parameters.Select(x=>x.GetType()).ToArray())(parameters);
    }

    public object Instantiate(params object[] parameters)
    {
      return TypeHelper.GetConstructorCall(_type, parameters.Select(x => x.GetType()).ToArray())(parameters);
    }
  }
}