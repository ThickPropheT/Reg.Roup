using System.Linq.Expressions;
using TreeVal.Eval;
using TreeVal.Scaffolding;

namespace TreeVal.Expr.Object;

// TODO
//  refine this api and add tests.
//  add overloads that expose the options supported.
public static class ConstructorEvaluatorExtensions
{
    public static IEvaluatorBuilder<NewExpression> New(this IEvaluatorBuilderFactory factory)
        => factory
            .OfType<NewExpression>()
            .Where(@new => @new.Constructor != null);
}
