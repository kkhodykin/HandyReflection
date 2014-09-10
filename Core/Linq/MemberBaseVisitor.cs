﻿using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using HandyReflection.Core.Annotations;
using HandyReflection.Core.Descriptors;
using HandyReflection.Core.Filters;
using Remotion.Linq;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Clauses.ExpressionTreeVisitors;
using Remotion.Linq.Parsing;
using Remotion.Linq.Parsing.ExpressionTreeVisitors;
using Remotion.Linq.Parsing.Structure;
using MemberFilter = HandyReflection.Core.Filters.MemberFilter;

namespace HandyReflection.Core.Linq
{

  class WhereClauseVisitor : ExpressionTreeVisitor
  {
    private readonly MemberFilterAggregator _aggregator;
    private readonly CompositeFilter _filter;

    public static Expression Visit(Expression expression, MemberFilterAggregator aggregator, CompositeFilter filter = null)
    {
      new WhereClauseVisitor(aggregator, filter).VisitExpression(expression);
    }

    public WhereClauseVisitor(MemberFilterAggregator aggregator, CompositeFilter filter)
    {
      _aggregator = aggregator;
      _filter = filter;
    }

    protected override Expression VisitBinaryExpression(BinaryExpression expression)
    {
      var filter = _aggregator.Push(expression, _filter);
      var left = Visit(expression.Left, _aggregator, filter);
      var right = Visit(expression.Right, _aggregator, filter);
      if (expression.Left != left || expression.Right != right)
        return Expression.MakeBinary(expression.NodeType, left, right);
      return expression;
    }

    protected override Expression VisitMemberExpression(MemberExpression expression)
    {
      _aggregator.Push(expression, _filter);
      return base.VisitMemberExpression(expression);
    }

    protected override Expression VisitConstantExpression(ConstantExpression expression)
    {
      _aggregator.Push(expression, _filter);
      return base.VisitConstantExpression(expression);
    }
  }

  class MemberBaseVisitor : QueryModelVisitorBase
  {
    readonly MemberFilterAggregator _filterAggregator = new MemberFilterAggregator();

    public static void Visit(QueryModel queryModel)
    {
      new MemberBaseVisitor().VisitQueryModel(queryModel);
    }

    public override void VisitWhereClause(WhereClause whereClause, QueryModel queryModel, int index)
    {
      WhereClauseVisitor.Visit(whereClause.Predicate, _filterAggregator);
      base.VisitWhereClause(whereClause, queryModel, index);
    }
  }

}