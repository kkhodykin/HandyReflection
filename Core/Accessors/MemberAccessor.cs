using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using HandyReflection.Core.Descriptors;
using HandyReflection.Core.Filters;
using HandyReflection.Core.Linq;
using Remotion.Linq;
using Remotion.Linq.Parsing.Structure;

namespace HandyReflection.Core.Accessors
{

	class MemberAccessor<TDescriptor> where TDescriptor : MemberDescriptorBase, new() // : QueryableBase<TDescriptor> //:IQueryProvider, IMemberAccessor<TDescriptor>
  {
    public object Instance { get; private set; }
		protected TDescriptor Descriptor { get; private set; }

		public MemberAccessor()
		{
			Descriptor = new TDescriptor();
		}

    public TAccessor SetInstance<TAccessor>(object instance) where TAccessor : IMemberAccessor<TDescriptor>
    {
      Instance = instance;
      return (TAccessor)(IMemberAccessor<TDescriptor>)this;
    }

    public IAccessor SetInstance(object instance)
    {
      throw new NotImplementedException();
    }

    public bool HasInstance()
    {
      return Instance != null;
    }
  }
}