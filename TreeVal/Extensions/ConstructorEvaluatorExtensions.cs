using System.Linq.Expressions;

namespace TreeVal.Extensions;

public static class ConstructorEvaluatorExtensions
{
    public static IEvaluatorBuilder<NewExpression> New(this IVisitorNodeFactory factory)
        => factory
            .OfType<NewExpression>()
            .Where(@new => @new.Constructor != null);
}
