using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using HandyReflection.Core;

namespace Leverate.Reflection
{
  /// <summary>
  /// Extensions to works with reflected properties.
  /// </summary>
  public static class PropertiesHelper
  {
    static readonly ConcurrentDictionary<PropertyInfo, Delegate> PropertyAccessorCache = new ConcurrentDictionary<PropertyInfo, Delegate>();
    static readonly ConcurrentDictionary<PropertyInfo, Delegate> PropertySetterCache = new ConcurrentDictionary<PropertyInfo, Delegate>();
    static readonly ConcurrentDictionary<PropertyInfo, Delegate> SimplePropertySetterCache = new ConcurrentDictionary<PropertyInfo, Delegate>();

    public static Action<TObject, TProperty> GetSetter<TObject, TProperty>(string propertyName)
    {
      var type = typeof(TObject);
      var property = type.GetPropertyInfo(propertyName);

      return property.GetSetter<TObject, TProperty>();
    }

    public static Action<object, object> GetSetter(this Type type, string propertyName)
    {
      var property = type.GetPropertyInfo(propertyName);
      return property.GetSetter();
    }

    public static PropertyInfo GetPropertyInfo(this Type type, string propertyName)
    {
      return MemberCache.Default.Get<PropertyInfo>(type, propertyName).SingleOrDefault();
    }

    /// <summary>
    /// Generates delegate that assigns supplied value to the property of supplied object
    /// </summary>
    /// <param name="property">PropertyInfo of the proper</param>
    /// <typeparam name="TObject"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    /// <returns></returns>
    public static Action<TObject, TProperty> GetSetter<TObject, TProperty>(this PropertyInfo property)
    {
      return (Action<TObject, TProperty>)PropertySetterCache.GetOrAdd(property, x => CreatePropertySetterExpression<TObject, TProperty>(property));
    }

    public static Action<object, object> GetSetter(this PropertyInfo property)
    {
      return
        (Action<object, object>)
          SimplePropertySetterCache.GetOrAdd(property, x => CreatePropertySetterExpression(property));
    }

    public static Func<TObject, TProperty> GetGetter<TObject, TProperty>(this Type type, string propertyName)
    {
      var property = MemberCache.Default.Get<PropertyInfo>(type, propertyName).FirstOrDefault();

      return property.GetGetter<TObject, TProperty>();
    }

    /// <summary>
    /// Generates delegate that returns property value of TObject.
    /// </summary>
    /// <typeparam name="TObject">Type of the property container object</typeparam>
    /// <typeparam name="TProperty">Type of the property</typeparam>
    /// <param name="property"></param>
    /// <returns></returns>
    public static Func<TObject, TProperty> GetGetter<TObject, TProperty>(this PropertyInfo property)
    {
      return (Func<TObject, TProperty>)PropertyAccessorCache.GetOrAdd(property, x => CreatePropertyAccessorExpression<TObject, TProperty>(property).Compile());
    }

    /// <summary>
    /// Generates x => x.PropertyName
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="property"></param>
    /// <returns></returns>
    public static Expression<Func<TObject, TProperty>> CreatePropertyAccessorExpression<TObject, TProperty>(this PropertyInfo property)
    {
      var paramExpression = Expression.Parameter(typeof(TObject), "x");
      Expression targetExpression = paramExpression;
      if (typeof (TObject) != property.ReflectedType)
        targetExpression = Expression.Convert(paramExpression, property.ReflectedType);
      var accessorExpression = Expression.Property(targetExpression, property);
      return Expression.Lambda<Func<TObject, TProperty>>(accessorExpression, paramExpression);
    }



    /// <summary>
    /// Generates x => x.PropertyName = val
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="property"></param>
    /// <returns></returns>
    static Action<TObject, TProperty> CreatePropertySetterExpression<TObject, TProperty>(PropertyInfo property)
    {
      var valueParamExpression = Expression.Parameter(typeof(TProperty), "val");
      var paramExpression = Expression.Parameter(typeof(TObject), "x");
      var propertyExpression = Expression.Property(paramExpression, property);

      var assign = Expression.Assign(propertyExpression, valueParamExpression);
      return Expression.Lambda<Action<TObject, TProperty>>(assign, paramExpression, valueParamExpression).Compile();
    }


    static Action<object, object> CreatePropertySetterExpression(PropertyInfo property)
    {
      var valueParamExpression = Expression.Parameter(typeof(object), "val");
      var paramExpression = Expression.Parameter(typeof(object), "x");
      var castParam = Expression.Convert(paramExpression, property.DeclaringType);
      var castValue = Expression.Convert(valueParamExpression, property.PropertyType);
      var propertyExpression = Expression.Property(castParam, property);

      var assign = Expression.Assign(propertyExpression, castValue);
      return Expression.Lambda<Action<object, object>>(assign, paramExpression, valueParamExpression).Compile();
    }


    public static T GetNestedPropertyByType<T>(object returnValue)
      where T : class
    {
      if (returnValue.GetType() == typeof(T))
      {
        return (T)returnValue;
      }

      return GetNestedProperty<T>(returnValue);
    }

    private static T GetNestedProperty<T>(object currentObject)
      where T : class
    {
      if (currentObject == null)
      {
        return null;
      }
      var currentType = currentObject.GetType();
      if (!currentType.IsClass || currentObject is IEnumerable)
      {
        return null;
      }

      foreach (var propertyInfo in currentType.GetProperties())
      {
        if (propertyInfo.PropertyType == typeof(T))
        {
          return (T)propertyInfo.GetValue(currentObject, null);
        }

        var propertyValue = propertyInfo.GetValue(currentObject, null);
        var resultInfo = GetNestedProperty<T>(propertyValue);
        if (resultInfo != null)
        {
          return resultInfo;
        }
      }

      return null;
    }
  }
}