using TreeVal.Eval.Condition;
using TreeVal.Media;
using TreeVal.Scaffolding;

namespace TreeVal.Eval;

public class OneOfNodeEvaluator : NodeEvaluator
{
    private readonly INodeEvaluatorFactory[] _options;

    public OneOfNodeEvaluator(
        IEnumerable<Func<Node, IEnumerable<ICondition>>> conditionLookups,
        INodeEvaluatorFactory[] options
    )
        : base(conditionLookups, [])
    {
        _options = options;
        ChildEvaluationStrategy = VisitationContext.EvaluationStrategy.OneOf;
    }

    public override IEnumerable<INodeEvaluatorFactory> EnumerateChildren(Node _) => _options;
}
