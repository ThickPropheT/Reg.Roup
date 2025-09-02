using TreeVal.Condition;
using TreeVal.Media;

namespace TreeVal;

public class OneOfNodeEvaluator : NodeEvaluator
{
    private readonly INodeEvaluatorFactory[] _options;

    public OneOfNodeEvaluator(IEnumerable<ICondition> conditions, INodeEvaluatorFactory[] options)
        : base(conditions, [])
    {
        _options = options;
        ChildEvaluationStrategy = VisitationContext.EvaluationStrategy.OneOf;
    }

    public override IEnumerable<INodeEvaluatorFactory> EnumerateChildren(Node _) => _options;
}
