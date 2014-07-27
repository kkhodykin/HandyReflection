using System;
using HandyReflection.Core.Exceptions;
using HandyReflection.Core.Extensions;
using HandyReflection.Core.Tests.SampleClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HandyReflection.Core.Tests
{
  [TestClass]
  public class PropertyExtensionsTests
  {
    private Parent _parent;
    private Child _child;
    private GrandChild _grandChild;

    [TestInitialize]
    public void Initialize()
    {
      _parent = new Parent();
      _child = new Child();
      _grandChild = new GrandChild();
    }

    [TestMethod]
    public void GetExistingProperty()
    {
      _parent.PublicIntProperty = 1;
      _parent.PublicStringProperty = "string";
      
      Assert.AreEqual(_parent.Reflect().Property("PublicIntProperty").Get(), 1);
      Assert.AreEqual(_parent.Reflect().Property("PublicStringProperty").Get(), "string");
      Assert.AreEqual(_parent.Reflect().Property("PublicIntProperty").Get<int>(), 1);
      Assert.AreEqual(_parent.Reflect().Property("PublicStringProperty").Get<string>(), "string");
    }


    [TestMethod]
    [ExpectedException(typeof(TypeMismatchException), AllowDerivedTypes = true)]
    public void GetExistingPropertyGenericWithWrongType_WithException()
    {
      _parent.Reflect().Property("PublicStringProperty").Get<int>();
    }


    [TestMethod]
    [ExpectedException(typeof(PropertyNotFoundException))]
    public void GetNonExistingProperty_WithException()
    {
      _parent.Reflect().Property("AbsentProperty").Get();
    }

    [TestMethod]
    [ExpectedException(typeof(PropertyNotFoundException))]
    public void GetNonExistingPropertyGeneric_WithException()
    {
      _parent.Reflect().Property("AbsentProperty").Get<int>();
    }

    [TestMethod]
    public void SetExistingProperty()
    {
      _parent.Reflect().Property("PublicStringProperty").Set("test");
      Assert.AreEqual(_parent.PublicStringProperty, "test");

      _child.Reflect().Property("PublicStringProperty").Set("test");
      Assert.AreEqual(_child.PublicStringProperty, "test");
    }

    [TestMethod]
    [ExpectedException(typeof(TypeMismatchException))]
    public void SetExistingPropertyWithWrongType_WithException()
    {
      _parent.Reflect().Property("PublicStringProperty").Set(1);
    }

    [TestMethod]
    [ExpectedException(typeof(MemberNotFoundException))]
    public void SetGetOnlyProperty_WithException()
    {
    }

    [TestMethod]
    public void SetPropertyWithPrivateSet()
    {
    }

    [TestMethod]
    [ExpectedException(typeof(PropertyNotFoundException), AllowDerivedTypes = true)]
    public void SetNonExistingProperty_WithException()
    {
    }

    [TestMethod]
    public void SetExistingPropertyGeneric()
    {
    }

    [TestMethod]
    [ExpectedException(typeof(PropertyNotFoundException))]
    public void SetNonExistingPropertyGeneric_WithException()
    {
      
    }
  }
}
