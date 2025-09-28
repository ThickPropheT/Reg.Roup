using System.Diagnostics;
using TreeVal.Diagnostics;
using TreeVal.Media;
using TreeVal.Stage.Eval;

namespace TreeVal.Eval;

[DebuggerDisplay("{Status} -> {Evaluator}")]
public class DefaultConditionEvaluation : IConditionEvaluation
{
    private EvaluationStatus? _status;

    public INodeEvaluation Owner { get; }

    public ICondition Evaluator { get; }
    public Node Target { get; }

    public EvaluationStatus Status => _status ?? EvaluationStatus.Accepted;

    public Exception? Error { get; private set; }

    public DefaultConditionEvaluation(INodeEvaluation parent, ICondition evaluator, Node target)
    {
        Owner = parent;
        Evaluator = evaluator;
        Target = target;
    }

    public void Reject(Exception? error = null)
    {
        System.Diagnostics.Debug.Assert(
            _status == null,
            $"Should mutable status be allowed? {Status} -> Rejected");

        Error = error;

        _status = EvaluationStatus.Rejected;
    }

    public void Describe(IDescriptionBuilder descriptionBuilder)
    {
        if (Status == EvaluationStatus.Accepted)
        {
            descriptionBuilder.EmitAcceptance(Evaluator);
        }
        else if (Status == EvaluationStatus.Rejected)
        {
            descriptionBuilder.EmitRejection(Evaluator, Target, Error);
        }
    }
}
