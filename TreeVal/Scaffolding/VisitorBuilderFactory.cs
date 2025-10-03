using System.Runtime.CompilerServices;
using TreeVal.Media;
using TreeVal.Scaffolding.Stage;
using TreeVal.Scaffolding.Stage.Create;
using TreeVal.Stage.Children;
using TreeVal.Stage.Eval;
using TreeVal.Stage.Eval.OfType;
using TreeVal.Stage.Eval.OneOf;
using TreeVal.Stage.Eval.Where;

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
            .ChildrenStage()
            .Create()
            .OrUpdate(
                (_, childrenStage) => childrenStage.AddChildren(() => options),
                _ => new OneOfConditionsStageBuilder()
            );

        return builder;
    }
}
