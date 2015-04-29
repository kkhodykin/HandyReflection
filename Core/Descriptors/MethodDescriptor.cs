using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HandyReflection.Core.Descriptors
{
  public class MethodDescriptor : MemberDescriptorBase
  {
	  private readonly IList<object> _parameters;
	  public Type ReturnType { get; internal set; }

	  public MethodDescriptor()
	  {
		  MemberTypes = MemberTypes.Method;
			_parameters = new List<object>();
		  ReturnType = typeof(void);
	  }

	  public IEnumerable<object> Parameters
	  {
		  get { return _parameters; }
	  }

	  public void AddParam<TParam>(TParam value)
	  {
		  _parameters.Add(value);
	  }
  }
}