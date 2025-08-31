using System.Linq.Expressions;
using TreeVal.Condition;

namespace TreeVal;

public class TypalEvaluatorBuilder : EvaluatorBuilder
{
    public ExpressionType NodeType { get; }

    public TypalEvaluatorBuilder(ExpressionType nodeType)
    {
        NodeType = nodeType;
        AddCondition(NodeTypeCondition.RejectNonMatching(nodeType));
    }
}

public class TypalEvaluatorBuilder<TExpression> : EvaluatorBuilder, IEvaluatorBuilder<TExpression>
    where TExpression : Expression
{
    public ExpressionType? NodeType { get; }

    public TypalEvaluatorBuilder(ExpressionType? nodeType)
    {
        NodeType = nodeType;
        AddCondition(NodeTypeCondition.AssertMatching<TExpression>(nodeType));
    }

    public void AddChildren(Func<TExpression, IEnumerable<IEvaluatorNodeFactory>> getChildren)
        => base.AddChildren(e => getChildren((TExpression) e));
}
