using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandyReflection.Core.Accessors
{
  public interface IAccessorProvider
  {
    TAccessor GetAccessor<TAccessor>();
  }

  internal class AccessorProvider : IAccessorProvider
  {
    static readonly Dictionary<Type, Func<object>> Accessors = new Dictionary<Type, Func<object>>
    {
      {typeof(IPropertyAccessor), ()=>new PropertyAccessor()},
      {typeof(IMethodAccessor), ()=>new MethodAccessor()},
      {typeof(IMemberAccessor), ()=>new MemberAccessor()},
    };

    public TAccessor GetAccessor<TAccessor>()
    {
      if(!Accessors.ContainsKey(typeof(TAccessor)))
        throw new ArgumentException("Not supported accessor type");

      return (TAccessor)Accessors[typeof (TAccessor)]();
    }
  }
}
