using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HandyReflection.Core.Descriptors
{
  public enum MemberVisibility
  {
    Public, NonPublic
  }

  public enum MemberBelonging
  {
    Instance, Static
  }

  public abstract class ReflectionDescriptorBase
  {
    public MemberVisibility Visibility { get; set; }
    public MemberBelonging Belonging { get; set; }
  }

  public abstract class MemberDescriptorBase : ReflectionDescriptorBase
  {
    public string Name { get; set; }
    public Type Type { get; set; }
  }

  public class MemberDescriptor : MemberDescriptorBase
  {
    public MemberInfo MemberInfo { get; set; }
  }
}
