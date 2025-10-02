using TreeVal.Diagnostics;
using TreeVal.Media;
using TreeVal.Stage.Eval;

namespace TreeVal.Visit;

public class Observer : ICondition, IDescribable
{
    private readonly Action<object, IConditionContext> _observe;

    public string? Label { get; init; }

    public Observer(Action<object, IConditionContext> observe)
    {
        _observe = observe;
    }

    public void Evaluate(Node node, IConditionContext context)
        => _observe(node.Value, context);

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
    private readonly Action<T, IConditionContext> _observe;

    public string? Label { get; init; }

    public Observer(Action<T, IConditionContext> observe)
    {
        _observe = observe;
    }

    public void Evaluate(Node node, IConditionContext context)
    {
        if (node.Value is not T t)
        {
            throw ConditionFailedException.ExpectedNode<T>(context);
        }

        _observe(t, context);
    }

    public void Evaluate(Node<T> node, IConditionContext context)
        => _observe(node.Value, context);

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
