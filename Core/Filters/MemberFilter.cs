using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using HandyReflection.Core.Descriptors;

namespace HandyReflection.Core.Filters
{
  enum FilterType
  {
    None, And, Or
  }
  class MemberFilter : ExpressionVisitor
  {
    enum ItemType
    {
      None, Belonging, Visibility
    }

    public BindingFlags BindingFlags { get; private set; }
    public String Name { get; private set; }
    //   public IEnumerable<Type> AtributeTypes { get; private set; }
    private ItemType _itemType = ItemType.None;

    readonly Stack<BindingFlags> _flags = new Stack<BindingFlags>();
    readonly Stack<FilterType> _filterTypes = new Stack<FilterType>();

    public void PushFilterItem(Type itemType)
    {
      if (itemType == typeof(MemberBelonging))
        _itemType = ItemType.Belonging;
      if (itemType == typeof(MemberVisibility))
        _itemType = ItemType.Visibility;
    }

    public void BeginFilter(FilterType type)
    {
      _filterTypes.Push(type);
    }

    public void EndFilter()
    {
      if (_filterTypes.Count > _flags.Count)
        _filterTypes.Pop();
    }

    public void PushFilterItemValue(int value)
    {
      switch (_itemType)
      {
        case ItemType.Belonging:
          _flags.Push(UpdateBindingFlags((MemberBelonging)value));
          break;
        case ItemType.Visibility:
          _flags.Push(UpdateBindingFlags((MemberVisibility)value));
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }

      if (_filterTypes.Count < _flags.Count)
      {
        _filterTypes.Push(FilterType.Or);
      }
    }

    BindingFlags UpdateBindingFlags(MemberBelonging belonging)
    {
      switch (belonging)
      {
        case MemberBelonging.Instance:
          return BindingFlags.Instance;
        case MemberBelonging.Static:
          return BindingFlags.Static;
      }
      throw new Exception();
    }

    BindingFlags UpdateBindingFlags(MemberVisibility visibility)
    {
      switch (visibility)
      {
        case MemberVisibility.Public:
          return BindingFlags.Public;
        case MemberVisibility.NonPublic:
          return BindingFlags.NonPublic;
      }
      throw new Exception();
    }
  }
}
