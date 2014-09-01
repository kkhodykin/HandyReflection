using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HandyReflection.Core.Extensions;
using HandyReflection.Core.Tests.SampleClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HandyReflection.Core.Tests
{
  [TestClass]
  public class TypeExtensionsTests
  {
    [TestMethod]
    public void ConstructClassNoPrameter()
    {
      var parent = typeof (Parent).ReflectType().Instantiate<Parent>();
      Assert.IsNotNull(parent);
      Assert.AreEqual(parent.PublicGetOnlyProperty, "PublicGetOnlyProperty");
    }

    [TestMethod]
    public void ConstructClassWithPrameter()
    {
      var parent = typeof(Parent).ReflectType().Instantiate<Parent>("TestProperty");
      Assert.IsNotNull(parent);
      Assert.AreEqual(parent.PublicStringProperty, "TestProperty");
    }
  }
}
