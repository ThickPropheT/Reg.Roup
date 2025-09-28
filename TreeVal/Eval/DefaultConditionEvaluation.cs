using System.Diagnostics;
using TreeVal.Diagnostics;
using TreeVal.Media;
using TreeVal.Stage.Eval;
using TreeVal.Visit.Behavior;

namespace TreeVal.Eval;

[DebuggerDisplay("{Status} -> {Condition}")]
public class DefaultConditionEvaluation : IConditionEvaluation
{
    private EvaluationStatus? _status;

    public IBehaviorContext BehaviorContext { get; }

    public ICondition Condition { get; }
    public Node Target { get; }

    public EvaluationStatus Status => _status ?? EvaluationStatus.Accepted;

    public Exception? Error { get; private set; }

    public DefaultConditionEvaluation(IBehaviorContext behaviorContext, ICondition condition, Node target)
    {
        BehaviorContext = behaviorContext;
        Condition = condition;
        Target = target;
    }

    public void Reject(Exception? error = null)
    {
        System.Diagnostics.Debug.Assert(
            _status == null,
            $"Should mutable status be allowed? {Status} -> Rejected");

        Error = error;

        _status = EvaluationStatus.Rejected;

        BehaviorContext.RecordResult(
            error != null
                ? ConditionEvaluationResult.ForError(this, error)
                : ConditionEvaluationResult.ForRejection(this)
        );
    }

    public void Describe(IDescriptionBuilder descriptionBuilder)
    {
        if (Status == EvaluationStatus.Accepted)
        {
            descriptionBuilder.EmitAcceptance(Condition);
        }
        else if (Status == EvaluationStatus.Rejected)
        {
            descriptionBuilder.EmitRejection(Condition, Target, Error);
        }
    }
}
