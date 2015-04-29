using System.Linq.Expressions;
using HandyReflection.Core.Filters;

namespace HandyReflection.Core.Linq
{

  class WhereClauseVisitor : ExpressionVisitor
  {
    private readonly MemberFilterAggregator _aggregator;
    private readonly FilterGroup _filter;

    public static Expression Visit(Expression expression, MemberFilterAggregator aggregator, FilterGroup filter = null)
    {
      return new WhereClauseVisitor(aggregator, filter).Visit(expression);
    }

    public WhereClauseVisitor(MemberFilterAggregator aggregator, FilterGroup filter)
    {
      _aggregator = aggregator;
      _filter = filter;
    }

    protected override Expression VisitBinary(BinaryExpression expression)
    {
      var filter = _aggregator.Push(expression, _filter);
      var left = Visit(expression.Left, _aggregator, filter);
      var right = Visit(expression.Right, _aggregator, filter);
      if (expression.Left != left || expression.Right != right)
        return Expression.MakeBinary(expression.NodeType, left, right);
      return expression;
    }

    protected override Expression VisitMember(MemberExpression expression)
    {
      _aggregator.Push(expression, _filter);
      return base.VisitMember(expression);
    }

    protected override Expression VisitConstant(ConstantExpression expression)
    {
      _aggregator.Push(expression, _filter);
      return base.VisitConstant(expression);
    }
  }

  class MemberBaseVisitor : ExpressionVisitor
  {
    readonly MemberFilterAggregator _filterAggregator = new MemberFilterAggregator();


    public static MemberCacheDescriptor GetCacheDescriptor(Expression expression)
    {
      var visitor = new MemberBaseVisitor();
      visitor.Visit(expression);
      return visitor._filterAggregator.ToMemberCacheDescriptor();

    }

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
      if (node.Method.Name == "Where")
    {
        WhereClauseVisitor.Visit(node.Arguments[1], _filterAggregator);
      }
      return base.VisitMethodCall(node);
    }
  }

  //class MemberBaseVisitor1 : QueryModelVisitorBase
  //{
  //  readonly MemberFilterAggregator _filterAggregator = new MemberFilterAggregator();
    
  //  public static MemberCacheDescriptor Visit(QueryModel queryModel)
  //  {
  //    var visitor = new MemberBaseVisitor();
  //    visitor.VisitQueryModel(queryModel);
  //    return visitor._filterAggregator.ToMemberCacheDescriptor();
  //  }

  //  public override void VisitWhereClause(WhereClause whereClause, QueryModel queryModel, int index)
  //  {
  //    WhereClauseVisitor.Visit(whereClause.Predicate, _filterAggregator);
  //    base.VisitWhereClause(whereClause, queryModel, index);
  //  }
}
