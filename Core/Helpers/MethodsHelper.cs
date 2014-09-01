using System;
using System.Linq;
using System.Reflection;

namespace HandyReflection.Core.Helpers
{
  public static class MethodsHelper
  {
    /// <summary>
    /// Search for a method by name and parameter types.  Unlike GetMethod(), does 'loose' matching on generic
    /// parameter types, and searches base interfaces.
    /// </summary>
    /// <exception cref="AmbiguousMatchException"/>
    public static MethodInfo FindMethod(this Type thisType, string name, params Type[] parameterTypes)
    {
      return FindMethod(thisType, name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, parameterTypes);
    }

    /// <summary>
    /// Search for a method by name, parameter types, and binding flags.  Unlike GetMethod(), does 'loose' matching on generic
    /// parameter types, and searches base interfaces.
    /// </summary>
    /// <exception cref="AmbiguousMatchException"/>
    public static MethodInfo FindMethod(this Type thisType, string name, BindingFlags bindingFlags, params Type[] parameterTypes)
    {
      var matchingMethod = FindMethodInternal(thisType, name, bindingFlags, parameterTypes);
      if (matchingMethod != null || !thisType.IsInterface) return matchingMethod;
      foreach (var interfaceType in thisType.GetInterfaces())
      {
        matchingMethod = FindMethodInternal(interfaceType, name, bindingFlags, parameterTypes);
        if(matchingMethod!= null)
          break;
      }

      return matchingMethod;
    }

    private static MethodInfo FindMethodInternal(Type type, string name, BindingFlags bindingFlags, params Type[] parameterTypes)
    {
      return MemberCache.Default.Get<MethodInfo>(new MemberCacheDescriptor(type, name)
      {
        BindingFlags = bindingFlags,
        MemberTypes = MemberTypes.Method
      })
        .SingleOrDefault(x => CheckArguments(parameterTypes, x));
    }

    internal static bool CheckArguments(Type[] argumentTypes, MethodBase methodInfo)
    {
      var parameterInfos = methodInfo.GetParameters();
      return parameterInfos.Length == argumentTypes.Length
             && !argumentTypes.Where((x, i) => !parameterInfos[i].ParameterType.IsSimilarType(x)).Any();
    }

    /// <summary>
    /// Special type used to match any generic parameter type in FindMethod().
    /// </summary>
    public class T
    { }

    /// <summary>
    /// Determines if the two types are either identical, or are both generic parameters or generic types
    /// with generic parameters in the same locations (generic parameters match any other generic paramter,
    /// but NOT concrete types).
    /// </summary>
    private static bool IsSimilarType(this Type thisType, Type type)
    {
      // Ignore any 'ref' types
      if (thisType.IsByRef)
        thisType = thisType.GetElementType();
      if (type.IsByRef)
        type = type.GetElementType();

      if (thisType.IsInterface && type.GetInterfaces().Contains(thisType))
        return true;

      // Handle array types
      if (thisType.IsArray && type.IsArray)
        return thisType.GetElementType().IsSimilarType(type.GetElementType());

      // If the types are identical, or they're both generic parameters or the special 'T' type, treat as a match
      if (thisType == type || ((thisType.IsGenericParameter || thisType == typeof(T)) && (type.IsGenericParameter || type == typeof(T))))
        return true;

      // Handle any generic arguments
      if (thisType.IsGenericType && type.IsGenericType && thisType.BaseType.IsAssignableFrom(type))
      {
        Type[] thisArguments = thisType.GetGenericArguments();
        Type[] arguments = type.GetGenericArguments();
        if (thisArguments.Length == arguments.Length)
        {
          for (int i = 0; i < thisArguments.Length; ++i)
          {
            if (!thisArguments[i].IsSimilarType(arguments[i]))
              return false;
          }
          return true;
        }
      }

      return false;
    }
  }
}