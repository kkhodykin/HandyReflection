using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Leverate.Reflection;

namespace HandyReflection.Core.Helpers
{
  public static class TypeHelper
  {
    /// <summary>
    /// Changes object type.
    /// </summary>
    /// <param name="value">Object which type requires changing</param>
    /// <param name="newType">Target type</param>
    /// <returns>New instance of target type converted from value</returns>
    /// <exception cref="ArgumentNullException">Can't process nested nullables</exception>
    public static object ChangeType(object value, Type newType)
    {
      var type = NormalizeType(newType);
      if (value == null)
      {
        if (IsTypeNullable(newType))
          return null;
        throw new ArgumentNullException("value");
      }

      return Convert.ChangeType(value, type);
    }

    private static Type NormalizeType(Type type)
    {
      if (IsTypeNullable(type))
      {
        return Nullable.GetUnderlyingType(type);
      }
      return type;
    }

    private static bool IsTypeNullable(Type type)
    {
      return type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>);
    }

    /// <summary>
    /// Gets all generic argument types for all class inheritance hierarchy.
    /// </summary>
    /// <param name="type">Target type</param>
    /// <param name="filter">Optional filter that is applied to the result</param>
    /// <param name="breakOnFirstEntry">Indicates whether to stop looking after first occurence</param>
    /// <returns></returns>
    public static IEnumerable<Type> TraverseGenericArguments(this Type type, Func<Type, bool> filter = null,
      bool breakOnFirstEntry = false)
    {
      var actualFilter = filter ?? (t => true);
      var currentArguments =
        (type.IsGenericType ? type.GetGenericArguments() : Enumerable.Empty<Type>()).Where(actualFilter).ToArray();

      if (currentArguments.Any() && breakOnFirstEntry)
        return currentArguments;

      return
        currentArguments.Union(type.BaseType != null
          ? TraverseGenericArguments(type.BaseType, filter, breakOnFirstEntry)
          : Enumerable.Empty<Type>());
    }

    private static readonly ConcurrentDictionary<string, Func<IEnumerable<object>, object>> ConstructorCalls =
      new ConcurrentDictionary<string, Func<IEnumerable<object>, object>>();


    public static Func<IEnumerable<object>, object> GetConstructorCall(Type type, params Type[] args)
    {
      var key = type.FullName + string.Join("$", args.Select(a => a.FullName));
      return ConstructorCalls.GetOrAdd(key,
        (Func<string, Func<IEnumerable<object>, object>>)(k => CreateConstructorCall(type, args)));
    }

    private static readonly MethodInfo ToArrayCall = MemberCache.Default.Get<MethodInfo>(new MemberCacheDescriptor(typeof(Enumerable), "ToArray")
    {
      BindingFlags = BindingFlags.Static | BindingFlags.Public,
      MemberTypes = MemberTypes.Method
    }).Single().MakeGenericMethod(typeof(object));

    private static Func<IEnumerable<object>, object> CreateConstructorCall(Type type, params Type[] args)
    {
      var descriptor = new MemberCacheDescriptor()
      {
        Type = type,
        BindingFlags = BindingFlags.Instance | BindingFlags.Public,
        Name = ".ctor",
        MemberTypes = MemberTypes.Constructor
      };
      var constructors = MemberCache.Default.Get<ConstructorInfo>(descriptor)
        .Where(ctor => MethodsHelper.CheckArguments(args, ctor)).ToList();

      if (!constructors.Any())
        throw new InvalidOperationException("No matching constructor found");
      if (constructors.Count() > 1)
        throw new InvalidOperationException("Ambigous matching constructors found");

      var constructor = constructors.Single();
      
      var argTypesExpression = Expression.Parameter(typeof (IEnumerable<object>));
      var toArray = Expression.Call(ToArrayCall, argTypesExpression);

      var paramExpressions = args.Select((x, i) =>
        Expression.Convert(
          Expression.ArrayAccess(toArray, Expression.Constant(i)), x));

      var ctorCall = Expression.Lambda<Func<IEnumerable<object>, object>>(
        //  (args)=>
        //    new (object)MyType(args[0], args[1], args[2]....)
        Expression.Convert(Expression.New(constructor, paramExpressions), typeof(object)),
        argTypesExpression);
      return ctorCall.Compile();
    }

  }
}