using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HandyReflection.Core.Accessors;
using HandyReflection.Core.Descriptors;

namespace HandyReflection.Core.Extensions
{
  public static class MethodExtensions
  {
    public static object CallFunc(this IQueryable<MethodDescriptor> source, params object[] arguments)
    {
      return source.GetAccessor<IMethodAccessor>().CallFunc(arguments);
    }

    public static TResult CallFunc<TResult>(this IQueryable<MethodDescriptor> source)
    {
      return source.GetAccessor<IMethodAccessor>().CallFunc<TResult>();
    }
    public static TResult CallFunc<T1, TResult>(this IQueryable<MethodDescriptor> source, T1 arg1)
    {
      return source.GetAccessor<IMethodAccessor>().CallFunc<T1, TResult>(arg1);
    }
    public static TResult CallFunc<T1, T2, TResult>(this IQueryable<MethodDescriptor> source, T1 arg1, T2 arg2)
    {
      return source.GetAccessor<IMethodAccessor>().CallFunc<T1, T2, TResult>(arg1, arg2);
    }
    public static TResult CallFunc<T1, T2, T3, TResult>(this IQueryable<MethodDescriptor> source, T1 arg1, T2 arg2, T3 arg3)
    {
      return source.GetAccessor<IMethodAccessor>().CallFunc<T1, T2, T3, TResult>(arg1, arg2, arg3);
    }
    public static TResult CallFunc<T1, T2, T3, T4, TResult>(this IQueryable<MethodDescriptor> source, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
      return source.GetAccessor<IMethodAccessor>().CallFunc<T1, T2, T3, T4, TResult>(arg1, arg2, arg3, arg4);
    }
    public static TResult CallFunc<T1, T2, T3, T4, T5, TResult>(this IQueryable<MethodDescriptor> source, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
      return source.GetAccessor<IMethodAccessor>().CallFunc<T1, T2, T3, T4, T5, TResult>(arg1, arg2, arg3, arg4, arg5);
    }

    public static void CallAction(this IQueryable<MethodDescriptor> source, params object[] arguments)
    {
      source.GetAccessor<IMethodAccessor>().CallAction(arguments);
    }
    public static void CallAction<T1>(this IQueryable<MethodDescriptor> source, T1 arg1)
    {
      source.GetAccessor<IMethodAccessor>().CallAction<T1>(arg1);
    }
    public static void CallAction<T1, T2>(this IQueryable<MethodDescriptor> source, T1 arg1, T2 arg2)
    {
      source.GetAccessor<IMethodAccessor>().CallAction<T1, T2>(arg1, arg2);
    }
    public static void CallAction<T1, T2, T3>(this IQueryable<MethodDescriptor> source, T1 arg1, T2 arg2, T3 arg3)
    {
      source.GetAccessor<IMethodAccessor>().CallAction<T1, T2, T3>(arg1, arg2, arg3);
    }
    public static void CallAction<T1, T2, T3, T4>(this IQueryable<MethodDescriptor> source, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
      source.GetAccessor<IMethodAccessor>().CallAction<T1, T2, T3, T4>(arg1, arg2, arg3, arg4);
    }
    public static void CallAction<T1, T2, T3, T4, T5>(this IQueryable<MethodDescriptor> source, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
      source.GetAccessor<IMethodAccessor>().CallAction<T1, T2, T3, T4, T5>(arg1, arg2, arg3, arg4, arg5);
    }

  }
}
