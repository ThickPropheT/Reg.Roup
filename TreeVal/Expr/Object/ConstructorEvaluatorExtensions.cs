using System.Linq.Expressions;
using TreeVal.Scaffolding;
using TreeVal.Stage.Eval.Where;

namespace TreeVal.Expr.Object;

// TODO
//  refine this api and add tests.
//  add overloads that expose the options supported.
public static class ConstructorEvaluatorExtensions
{
    public static IVisitorBuilder<NewExpression> New(this IVisitorBuilderFactory factory)
        => factory
            .OfType<NewExpression>()
            .Where(@new => @new.Constructor != null);
}
