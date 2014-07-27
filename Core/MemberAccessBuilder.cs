using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HandyReflection.Core.Accessors;
using HandyReflection.Core.Descriptors;

namespace HandyReflection.Core
{
  public interface IMemberAccessBuilder
  {
    IQueryable<MethodDescriptor> Method(string methodName);
    IQueryable<MethodDescriptor> Methods();
    IQueryable<PropertyDescriptor> Property(string propertyName);
    IQueryable<PropertyDescriptor> Properties();
    IQueryable<MemberDescriptor> Member(string memberName);
    IQueryable<MemberDescriptor> Members();
  }

  internal class MemberAccessBuilder:IMemberAccessBuilder
  {
    private readonly object _instance;

    public MemberAccessBuilder()
    {
      
    }

    public MemberAccessBuilder(object instance)
    {
      _instance = instance;
    }

    public IQueryable<MethodDescriptor> Method(string methodName)
    {
      return new MethodAccessor(_instance).Where(x => x.Name == methodName);
    }

    public IQueryable<MethodDescriptor> Methods()
    {
      return new MethodAccessor(_instance);
    }

    public IQueryable<PropertyDescriptor> Property(string propertyName)
    {
      return new PropertyAccessor(_instance).Where(x => x.Name == propertyName);
    }

    public IQueryable<PropertyDescriptor> Properties()
    {
      return new PropertyAccessor(_instance);
    }

    public IQueryable<MemberDescriptor> Member(string memberName)
    {
      return new MemberAccessor(_instance).Where(x => x.Name == memberName);
    }

    public IQueryable<MemberDescriptor> Members()
    {
      return new MemberAccessor(_instance);
    }
  }
}
