using System.Linq.Expressions;

namespace TreeVal.Extensions;

public static class DebugEvaluatorExtensions
{
    public static IEvaluatorBuilder<Expression> Debug(
        this IVisitorNodeFactory factory, Action<Expression> observe, bool? @break = null)
        => factory.Where(e =>
        {
            observe(e);
            var success = @break != true;
            return success;
        });

    public static IEvaluatorBuilder<Expression> Debug(
        this IEvaluatorBuilder<Expression> factory, Action<Expression> observe, bool? @break = null)
        => factory.Where(e =>
        {
            observe(e);
            var success = @break != true;
            return success;
        });
}
