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

public class TypalEvaluatorBuilder<TNode> : EvaluatorBuilder, IEvaluatorBuilder<TNode>
    where TNode : Expression
{
    public ExpressionType? NodeType { get; }

    public TypalEvaluatorBuilder(ExpressionType? nodeType)
    {
        NodeType = nodeType;
        AddCondition(NodeTypeCondition.AssertMatching<TNode>(nodeType));
    }
}
