using System.Linq.Expressions;
using TreeVal.Condition;

namespace TreeVal;

public class OneOfEvaluatorNode : EvaluatorNode
{
    private readonly IEvaluatorNodeFactory[] _options;

    public OneOfEvaluatorNode(IEnumerable<ICondition> conditions, IEvaluatorNodeFactory[] options)
        : base(conditions, [])
    {
        _options = options;
        ChildEvaluationStrategy = VisitationContext.EvaluationStrategy.OneOf;
    }

    public override IEnumerable<IEvaluatorNodeFactory> EnumerateChildren(Expression _) => _options;
}
