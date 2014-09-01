using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandyReflection.Core.Tests.SampleClasses
{
  interface IParent
  {
    string PublicStringProperty { get; set; }
    int PublicIntProperty { get; set; }
    string PublicGetProperty { get; }
    string PublicGetOnlyProperty { get; }
  }

  class Parent : IParent
  {
    public string PublicStringProperty { get; set; }
    protected string ProtectedStringProperty { get; set; }
    private string PrivateStringProperty { get; set; }

    public int PublicIntProperty { get; set; }
    public string PublicGetProperty { get; private set; }
    public string PublicGetOnlyProperty {
      get { return "PublicGetOnlyProperty"; }
    }
    protected int ProtectedIntProperty { get; set; }
    private int PrivateIntProperty { get; set; }

    public Parent()
    {
      
    }

    public Parent(string stringProp)
    {
      PublicStringProperty = stringProp;
    }
  }
}
