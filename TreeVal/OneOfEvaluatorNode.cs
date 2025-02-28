using System.Linq.Expressions;
using TreeVal.Condition;

namespace TreeVal;

public class OneOfEvaluatorNode : EvaluatorNode
{
    private readonly IEvaluatorNodeFactory[] _options;

    public override VisitationContext.EvaluationStrategy ChildEvaluationStrategy 
        => VisitationContext.EvaluationStrategy.OneOf;

    public OneOfEvaluatorNode(IEnumerable<ICondition> conditions, IEvaluatorNodeFactory[] options)
        : base(conditions, [])
    {
        _options = options;
    }

    public override IEnumerable<IEvaluatorNodeFactory> EnumerateChildren(Expression _) => _options;
}
