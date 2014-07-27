using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using HandyReflection.Core.Descriptors;
using HandyReflection.Core.Filters;
using MemberFilter = HandyReflection.Core.Filters.MemberFilter;

namespace HandyReflection.Core.Expressions
{
  class MemberBaseVisitor : ExpressionVisitor
  {
    enum State
    {
      Idle, FilterFound, SearchingValue, FilterUpdated
    }

    State _state = State.Idle;

    public MemberFilter Filter { get; private set; }

    public MemberBaseVisitor()
    {
      Filter = new MemberFilter();
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
      Expression result;
      switch (node.NodeType)
      {
        case ExpressionType.Equal:
          result = TryUpdateFilter(node);
          break;
        case ExpressionType.OrElse:
          Filter.BeginFilter(FilterType.Or);
          result = base.VisitBinary(node);
          break;
        case ExpressionType.AndAlso:
          Filter.BeginFilter(FilterType.And);
          result = base.VisitBinary(node);
          Filter.EndFilter();
          break;
        default:
          result = base.VisitBinary(node);
          break;
      }
      return result;
    }

    private Expression TryUpdateFilter(BinaryExpression node)
    {
      Visit(node.Left);

      if (_state == State.FilterFound)
        _state = State.SearchingValue;

      Visit(node.Right);
      if (_state == State.FilterFound)
      {
        _state = State.SearchingValue;
        Visit(node.Left);
      }

      if (_state == State.SearchingValue)
      {
        throw new InvalidOperationException("Something went wrong");
      }
      if (_state == State.FilterUpdated)
        return node;

    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
      if (_state == State.SearchingValue && node.Type == typeof(int))
      {
        Filter.PushFilterItemValue((int)node.Value);
        _state = State.FilterUpdated;
      }
      return base.VisitConstant(node);
    }

    protected override Expression VisitMember(MemberExpression node)
    {
      var property = node.Member as PropertyInfo;
      if (property == null)
        return base.VisitMember(node);
      if (property.PropertyType == typeof(MemberBelonging))
      {
        _state = State.FilterFound;
        Filter.PushFilterItem(property.PropertyType);
      }
      return node;
    }

  }
}
