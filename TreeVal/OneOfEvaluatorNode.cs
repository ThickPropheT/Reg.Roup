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

    protected override void EvaluateChildren(VisitationContext context, Expression current)
    {
        var accepted = _options.FirstOrDefault(option =>
        { 
            var branch = context.CreateBranch();
            
            var visitor = option.ToEvaluator();
            visitor.Evaluate(branch);

            return branch.TryMerge();
        });

        if (accepted == null)
        {
            context.Reject(this);
        }
        else
        {
            // TODO add auto-accept and remove this
            context.Accept(this);
        }
    }
}
