using System;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using HandyReflection.Core.Accessors;
using HandyReflection.Core.Descriptors;
using HandyReflection.Core.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HandyReflection.Core.Tests
{
  /// <summary>
  /// Summary description for MemberTreeVisitorTests
  /// </summary>
  [TestClass]
  public class MemberTreeVisitorTests
  {
    [TestMethod]
    public void QueryInstanceBindingFlag()
    {

      var initialQuery = new MemberAccessor<MemberDescriptor>();
      //var query = initialQuery.Where(x => x.AccessMode == MemberAccessMode.Instance).ToList();

      var query1 =
        initialQuery.Where(
          x =>
            x.MemberTypes == MemberTypes.Property 
            && x.AccessMode == MemberAccessMode.Instance 
            && (x.Visibility == MemberVisibility.NonPublic || x.Visibility == MemberVisibility.Public)
            && x.Name == "Test"
            ).ToList();
    }
  }
}
