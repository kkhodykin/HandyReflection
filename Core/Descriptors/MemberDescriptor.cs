using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HandyReflection.Core.Descriptors
{
  public enum MemberVisibility
  {
    Any = 0, Public = 1, NonPublic = 2
  }

  public enum MemberAccessMode
  {
    Any = 0, Instance = 1, Static = 2
  }

  public abstract class ReflectionDescriptorBase
  {
    public MemberVisibility Visibility { get; internal set; }
    public MemberAccessMode AccessMode { get; internal set; }
    public IAttributesDescriptor Attributes { get; internal set; }
  }

  public abstract class MemberDescriptorBase : ReflectionDescriptorBase
  {
    public string Name { get; internal set; }
    public Type Type { get; internal set; }
    public MemberTypes MemberTypes { get; internal set; }
  }

  public class MemberDescriptor : MemberDescriptorBase
  {
    public MemberInfo MemberInfo { get; internal set; }
  }
}