using TreeVal.Diagnostics;
using TreeVal.Media;
using TreeVal.Stage.Eval;

namespace TreeVal.Visit;

public class Observer : ICondition, IDescribable
{
    private readonly Action<object, IConditionEvaluation> _observe;

    public string? Label { get; init; }

    public Observer(Action<object, IConditionEvaluation> observe)
    {
        _observe = observe;
    }

    public void Evaluate(Node node, IConditionEvaluation evaluation)
        => _observe(node.Value, evaluation);

    public void Describe(IDescriptionBuilder descriptionBuilder)
    {
        if (Label == null)
        {
            descriptionBuilder.EmitLine($"{this},");
            return;
        }

        descriptionBuilder.EmitBlock(() =>
        {
            descriptionBuilder.EmitLine($"Type: {this},");
            descriptionBuilder.EmitLine($"Label: {Label},");
        });
    }
}

public class Observer<T> : ICondition<T>, IDescribable
{
    private readonly Action<T, IConditionEvaluation> _observe;

    public string? Label { get; init; }

    public Observer(Action<T, IConditionEvaluation> observe)
    {
        _observe = observe;
    }

    public void Evaluate(Node node, IConditionEvaluation evaluation)
    {
        if (node.Value is not T t)
        {
            throw ConditionFailedException.ExpectedNode<T>(evaluation);
        }

        _observe(t, evaluation);
    }

    public void Evaluate(Node<T> node, IConditionEvaluation evaluation)
        => _observe(node.Value, evaluation);

    public void Describe(IDescriptionBuilder descriptionBuilder)
    {
        if (Label == null)
        {
            descriptionBuilder.EmitLine($"{this},");
            return;
        }

        descriptionBuilder.EmitBlock(() =>
        {
            descriptionBuilder.EmitLine($"Type: {this},");
            descriptionBuilder.EmitLine($"Label: {Label},");
        });
    }
}
