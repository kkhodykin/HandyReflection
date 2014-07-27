using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandyReflection.Core.Tests.SampleClasses
{
  internal interface IChild
  {
    string ChildPublicStringProperty { get; set; }
    int ChildPublicIntProperty { get; set; }
  }

  class Child : Parent, IChild
  {
    public string ChildPublicStringProperty { get; set; }
    protected string ChildProtectedStringProperty { get; set; }
    private string ChildPrivateStringProperty { get; set; }

    public int ChildPublicIntProperty { get; set; }
    protected int ChildProtectedIntProperty { get; set; }
    private int ChildPrivateIntProperty { get; set; }
  }
}
