using System;
using System.Collections.Generic;
using System.Linq;

namespace Leverate.Reflection
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
      return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
    }

    /// <summary>
    /// Gets all generic argument types for all class inheritance hierarchy.
    /// </summary>
    /// <param name="type">Target type</param>
    /// <param name="filter">Optional filter that is applied to the result</param>
    /// <param name="breakOnFirstEntry">Indicates whether to stop looking after first occurence</param>
    /// <returns></returns>
    public static IEnumerable<Type> TraverseGenericArguments(this Type type, Func<Type, bool> filter = null, bool breakOnFirstEntry = false)
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

  }
}
