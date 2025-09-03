using TreeVal.Diagnostics;
using TreeVal.Eval.Condition;
using TreeVal.Media;

namespace TreeVal.Eval;

public class Evaluation : IDescribable
{
    private Status? _status;

    public Status CurrentStatus => _status ?? Status.Accepted;

    public Exception? Error { get; private set; }

    public ICondition? Condition { get; private set; }
    public Node? Actual { get; private set; }

    public IEnumerable<Evaluation> ChildEvaluations { get; private set; } = [];

    public void Reject(IEnumerable<Evaluation> evaluated, Exception? error = null)
    {
        System.Diagnostics.Debug.Assert(
            _status == null,
            $"Should mutable status be allowed? {CurrentStatus} -> Rejected");

        Error = error;
        ChildEvaluations = evaluated;

        _status = Status.Rejected;
    }

    public void Reject(ICondition condition, Node actual, Exception? error = null)
    {
        System.Diagnostics.Debug.Assert(
            _status == null,
            $"Should mutable status be allowed? {CurrentStatus} -> Rejected");

        Error = error;
        Condition = condition;
        Actual = actual;

        _status = Status.Rejected;
    }

    public void Describe(IDescriptionBuilder descriptionBuilder)
    {
        if (Condition != null)
        {
            descriptionBuilder.EmitBlock(
                $"{nameof(Condition)}:",
                () =>
                {
                    descriptionBuilder.Emit("Expected:");
                    
                    // ReSharper disable once SuspiciousTypeConversion.Global
                    if (Condition is IDescribable d)
                    {
                        d.Describe(descriptionBuilder);
                    }
                    else
                    {
                        Condition.Describe(descriptionBuilder);
                    }

                    descriptionBuilder.Emit("Actual:");
                    if (Actual != null)
                    {
                        Actual.Describe(descriptionBuilder);
                    }
                    else
                    {
                        descriptionBuilder.EmitLine("null");
                    }
                });
        }

        var childEvaluations = ChildEvaluations.ToArray();

        if (childEvaluations.Any())
        {
            descriptionBuilder.EmitBlock(
                $"{nameof(ChildEvaluations)}:",
                () =>
                {
                    foreach (var childEvaluation in childEvaluations)
                    {
                        childEvaluation.Describe(descriptionBuilder);
                    }
                });
        }

        if (Error != null)
        {
            descriptionBuilder.EmitError(Error);
            descriptionBuilder.EmitNewline();
        }
    }

    public enum Status
    {
        Accepted,
        Rejected
    }
}
