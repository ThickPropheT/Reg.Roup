using System.Linq.Expressions;
using TreeVal.Eval;
using TreeVal.Scaffolding;

namespace TreeVal.Expr.Conversion;

public static class IgnoreBoxingEvaluatorExtensions
{
    public static IVisitorBuilderFactory IgnoreBoxing(this IVisitorBuilderFactory factory)
        => new BuilderFactoryAspect(
            factory,
            builder =>
            {
                builder
                    .Get<IReadNodeStageBuilder>()
                    .OrCreateStage((_, readStage) =>
                        readStage.AfterEntering(_ =>
                            new MediaBehavior.SkipWhile(n =>
                                n.Value is UnaryExpression { NodeType: ExpressionType.Convert }
                            ))
                    );

                return builder;
            });
}
