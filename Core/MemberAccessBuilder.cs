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

  internal class MemberAccessBuilder : IMemberAccessBuilder
  {
    private readonly IAccessorProvider _accessorProvider;
    private readonly object _instance;

    public MemberAccessBuilder(IAccessorProvider accessorProvider)
    {
      _accessorProvider = accessorProvider;
    }

    public MemberAccessBuilder(IAccessorProvider accessorProvider, object instance)
      : this(accessorProvider)
    {
      _instance = instance;
    }

    public IQueryable<MethodDescriptor> Method(string methodName)
    {
      return _accessorProvider.GetAccessor<IMethodAccessor>().Where(x => x.Name == methodName);
    }

    public IQueryable<MethodDescriptor> Methods()
    {
      return _accessorProvider.GetAccessor<IMethodAccessor>();
    }

    public IQueryable<PropertyDescriptor> Property(string propertyName)
    {
      return _accessorProvider.GetAccessor<IPropertyAccessor>().Where(x => x.Name == propertyName);
    }

    public IQueryable<PropertyDescriptor> Properties()
    {
      return _accessorProvider.GetAccessor<IPropertyAccessor>();
    }

    public IQueryable<MemberDescriptor> Member(string memberName)
    {
      return _accessorProvider.GetAccessor<IMemberAccessor<MemberDescriptor>>().Where(x => x.Name == memberName);
    }

    public IQueryable<MemberDescriptor> Members()
    {
      return _accessorProvider.GetAccessor<IMemberAccessor<MemberDescriptor>>();
    }
  }
}
