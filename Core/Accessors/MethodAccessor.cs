using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HandyReflection.Core.Descriptors;

namespace HandyReflection.Core.Accessors
{
  internal interface IMethodAccessor : IMemberAccessor<MethodDescriptor>
  {
    object CallFunc(params object[] arguments);
    TResult CallFunc<TResult>();
    TResult CallFunc<T1,TResult>(T1 arg1);
    TResult CallFunc<T1, T2, TResult>(T1 arg1, T2 arg2);
    TResult CallFunc<T1, T2, T3, TResult>(T1 arg1, T2 arg2, T3 arg3);
    TResult CallFunc<T1, T2, T3, T4, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
    TResult CallFunc<T1, T2, T3, T4, T5, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

    void CallAction(params object[] arguments);
    void CallAction<T1>(T1 arg1);
    void CallAction<T1, T2>(T1 arg1, T2 arg2);
    void CallAction<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3);
    void CallAction<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
    void CallAction<T1, T2, T3, T4, T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

  }

  class MethodAccessor : MemberAccessor<MethodDescriptor>, IMethodAccessor
  {
    public object CallFunc(params object[] arguments)
    {
      throw new NotImplementedException();
    }

    public TResult CallFunc<TResult>()
    {
      throw new NotImplementedException();
    }

    public TResult CallFunc<T1, TResult>(T1 arg1)
    {
      throw new NotImplementedException();
    }

    public TResult CallFunc<T1, T2, TResult>(T1 arg1, T2 arg2)
    {
      throw new NotImplementedException();
    }

    public TResult CallFunc<T1, T2, T3, TResult>(T1 arg1, T2 arg2, T3 arg3)
    {
      throw new NotImplementedException();
    }

    public TResult CallFunc<T1, T2, T3, T4, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
      throw new NotImplementedException();
    }

    public TResult CallFunc<T1, T2, T3, T4, T5, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
      throw new NotImplementedException();
    }

    public void CallAction(params object[] arguments)
    {
      throw new NotImplementedException();
    }

    public void CallAction<T1>(T1 arg1)
    {
      throw new NotImplementedException();
    }

    public void CallAction<T1, T2>(T1 arg1, T2 arg2)
    {
      throw new NotImplementedException();
    }

    public void CallAction<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3)
    {
      throw new NotImplementedException();
    }

    public void CallAction<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
      throw new NotImplementedException();
    }

    public void CallAction<T1, T2, T3, T4, T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
      throw new NotImplementedException();
    }
  }
}
