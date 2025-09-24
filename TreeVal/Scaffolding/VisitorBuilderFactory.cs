using System.Runtime.CompilerServices;
using TreeVal.Eval;
using TreeVal.Eval.Condition;
using TreeVal.Media;

namespace TreeVal.Scaffolding;

public class VisitorBuilderFactory : IVisitorBuilderFactory
{
    public IStageDirector StageDirector { get; }

    public VisitorBuilderFactory(IStageDirector director)
    {
        StageDirector = director;
    }

    public IVisitorBuilder Where(
        Func<Node, bool> predicate, [CallerArgumentExpression(nameof(predicate))] string predicateExpression = "")
        => DefaultVisitorBuilder
            .Create(this)
            .Where(predicate, predicateExpression);

    public IVisitorBuilder<TNode> OfType<TNode>()
    {
        var builder = DefaultVisitorBuilder.Create<TNode>(this);

        builder.AddCondition(NodeTypeCondition.AssertMatching<TNode>());

        return builder;
    }

    public IVisitorBuilder OneOf(
        IVisitorFactory option1, IVisitorFactory option2, params IVisitorFactory[] options)
    {
        var builder = DefaultVisitorBuilder.Create(this);
        options = new[] { option1, option2 }.Concat(options).ToArray();

        builder
            .Get<IVisitChildrenStageBuilder>()
            .OrCreateStage(
                (_, childrenStage) => childrenStage.AddChildren(() => options),
                _ => new OneOfConditionsStageBuilder()
            );

        return builder;
    }
}
