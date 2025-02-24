using System.Linq.Expressions;

namespace TreeVal;

public static class ConstructorVisitorNodeExtensions
{
    public static IEvaluatorBuilder<NewExpression> New(this IVisitorNodeFactory factory)
        => factory
            .OfType<NewExpression>()
            .Where(@new => @new.Constructor != null);
}
