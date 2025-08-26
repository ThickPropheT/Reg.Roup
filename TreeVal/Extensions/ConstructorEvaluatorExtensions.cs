using System.Linq.Expressions;

namespace TreeVal.Extensions;

// TODO
//  refine this api and add tests.
//  add overloads that expose the options supported.
public static class ConstructorEvaluatorExtensions
{
    public static IEvaluatorBuilder<NewExpression> New(this IVisitorNodeFactory factory)
        => factory
            .OfType<NewExpression>()
            .Where(@new => @new.Constructor != null);
}
