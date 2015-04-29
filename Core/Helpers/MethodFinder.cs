using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using HandyReflection.Core.Descriptors;

namespace HandyReflection.Core.Helpers
{
	class MethodFinder
	{
		MethodInfo FindMethod(MethodDescriptor descriptor)
		{
			var methodInfos = MemberCache.Default.Get<MethodInfo>(new MemberCacheDescriptor(descriptor));
			throw new NotImplementedException();
		}

		IEnumerable<MethodInfo> Reduce(IEnumerable<MethodInfo> currentSet, MethodDescriptor descriptor)
		{
			throw new NotImplementedException();
		}
		
		
	}
}
