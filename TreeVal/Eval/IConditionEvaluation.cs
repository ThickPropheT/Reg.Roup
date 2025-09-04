using TreeVal.Diagnostics;

namespace TreeVal.Eval;

public interface IConditionEvaluation : IDescribable
{
    EvaluationStatus Status { get; }

    void Reject(Exception? error = null);
}
