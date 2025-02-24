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
    }

    protected override void EvaluateChildren(IVisitationContext context, Expression current)
    {
        foreach (var option in _options)
        {
            var tracker = context.Try(copy =>
            {
                var visitor = option.ToEvaluator();
                visitor.Evaluate(copy);
            });

            if (!tracker.HasRejection)
            {
                context.Accept(this);
                return;
            }
        }

        // TODO does this really need to reject AND throw?
        context.Reject(this);
        throw new TreeRejectedException(nameof(OneOfEvaluatorNode));
    }
}
