using TreeVal.Diagnostics;
using TreeVal.Visit.Behavior;

namespace TreeVal.Stage.Eval;

public interface IConditionContext : IDescribable
{
    IBehaviorContext BehaviorContext { get; }

    EvaluationStatus Status { get; }

    void Reject(Exception? error = null);
}
