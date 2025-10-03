using System.Linq.Expressions;
using TreeVal.Scaffolding;
using TreeVal.Scaffolding.Stage.Get;
using TreeVal.Stage.Read;

namespace TreeVal.Expr.Conversion;

public static class IgnoreBoxingEvaluatorExtensions
{
    public static IVisitorBuilderFactory IgnoreBoxing(this IVisitorBuilderFactory factory)
        => new BuilderFactoryAspect(
            factory,
            builder =>
            {
                builder
                    .ReadStage()
                    .Get()
                    .OrCreate((_, readStage) =>
                        readStage.AfterEntering((_, _) =>
                            new MediaBehavior.SkipWhile(n =>
                                n.Value is UnaryExpression { NodeType: ExpressionType.Convert }
                            ))
                    );

                return builder;
            });
}
