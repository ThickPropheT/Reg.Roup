using TreeVal.Eval.Condition;
using TreeVal.Media;
using TreeVal.Scaffolding;

namespace TreeVal.Eval;

public class OneOfNodeEvaluator : Visitor
{
    private readonly IVisitorFactory[] _options;

    public OneOfNodeEvaluator(IEnumerable<ICondition> conditions, IVisitorFactory[] options)
        : base(conditions, [])
    {
        _options = options;
        ChildEvaluationStrategy = VisitationContext.EvaluationStrategy.OneOf;
    }

    public IEnumerable<IVisitorFactory> EnumerateChildren(Node _) => _options;
}
