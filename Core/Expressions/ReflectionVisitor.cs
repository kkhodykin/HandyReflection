using System.Linq.Expressions;
using HandyReflection.Core.Filters;

namespace HandyReflection.Core.Expressions
{
  internal class ReflectionVisitor<TVisitor> : ExpressionVisitor where TVisitor : MemberBaseVisitor, new()
  {
    readonly TVisitor _visitor = new TVisitor();

    public MemberFilter Filter { get { return _visitor.Filter; } }

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
      if (node.Method.Name != "Where" || node.Arguments.Count != 2)
        return node;

      var parsedPredicate = _visitor.Visit(node.Arguments[1]);
      if (ReferenceEquals(parsedPredicate, node.Arguments[1]))
        return node;

      var objectExpression = Visit(node.Object);
      if (parsedPredicate == null)
        return objectExpression;

      return Expression.Call(objectExpression, node.Method, node.Arguments[0], parsedPredicate);
    }
  }
}
