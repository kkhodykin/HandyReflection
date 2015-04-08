using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HandyReflection.Core.Descriptors;

namespace HandyReflection.Core.Accessors
{
	public interface IFunctionAccessor : IMethodAccessor
	{
		object GetResult();
		TResult GetResult<TResult>();
	}

	public interface IMethodAccessor : IMemberAccessor<MethodDescriptor>
	{
		IFunctionAccessor Returns(Type returnType);
		IFunctionAccessor Returns<TResult>();
		IMethodAccessor WithParam<TParameter>(TParameter value);
		void Call();
		//IMethodAccessor Generic(params Type[] genericParamTypes);
		//IMethodAccessor Generic<TParam>();
		//IMethodAccessor Generic<TParam1, TParam2>();
		//IMethodAccessor Generic<TParam1, TParam2, TParam3>();
	}

  class MethodAccessor : MemberAccessor<MethodDescriptor>, IFunctionAccessor
  {
	  public IFunctionAccessor Returns(Type returnType)
	  {
		  Descriptor.ReturnType = returnType;
		  return this;
	  }

	  public IFunctionAccessor Returns<TResult>()
	  {
		  return Returns(typeof (TResult));
	  }

	  public IMethodAccessor WithParam<TParameter>(TParameter value)
	  {
		  Descriptor.AddParam(value);
		  return this;
	  }

	  public void Call()
	  {
		  throw new NotImplementedException();
	  }

	  public object GetResult()
	  {
		  throw new NotImplementedException();
	  }

	  public TResult GetResult<TResult>()
	  {
		  throw new NotImplementedException();
	  }
  }
}
