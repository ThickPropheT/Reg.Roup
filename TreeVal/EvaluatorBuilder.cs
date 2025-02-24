using System.Linq.Expressions;
using TreeVal.Condition;

namespace TreeVal;

public abstract class EvaluatorBuilderBase : IEvaluatorBuilder
{
    private readonly List<ICondition> _conditions = new(1);
    private readonly List<Func<Expression, IEnumerable<IEvaluatorNodeFactory>>> _childLookups = new(1);

    protected EvaluatorBuilderBase()
    {
        AddCondition(new NotNullCondition());
    }

    public virtual IEvaluatorNode ToEvaluator()
        => new EvaluatorNode(_conditions.ToArray(), _childLookups);

    public void AddCondition(ICondition condition)
        => _conditions.Add(condition);

    public void AddChildren(Func<Expression, IEnumerable<IEvaluatorNodeFactory>> getChildren)
        => _childLookups.Add(getChildren);
}

public class EvaluatorBuilder : EvaluatorBuilderBase
{
    public ExpressionType NodeType { get; }

    public EvaluatorBuilder(ExpressionType nodeType)
    {
        NodeType = nodeType;
        AddCondition(NodeTypeCondition.RejectNonMatching(nodeType));
    }
}

public class EvaluatorBuilder<TNode> : EvaluatorBuilderBase, IEvaluatorBuilder<TNode>
    where TNode : Expression
{
    public ExpressionType? NodeType { get; }

    public EvaluatorBuilder(ExpressionType? nodeType)
    {
        NodeType = nodeType;
        AddCondition(NodeTypeCondition.AssertMatching<TNode>(nodeType));
    }
}
