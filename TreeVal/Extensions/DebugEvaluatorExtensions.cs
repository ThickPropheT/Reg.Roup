using System.Linq.Expressions;

namespace TreeVal.Extensions;

public static class DebugEvaluatorExtensions
{
    public static IEvaluatorBuilder Debug(this VisitorNodeFactory factory, Action<Expression> observe,
        bool? @break = null)
        => factory.Where(e =>
        {
            observe(e);
            var success = @break != true;
            return success;
        });

    public static TBuilder Debug<TBuilder>(this TBuilder factory, Action<Expression> observe, bool? @break = null)
        where TBuilder : IEvaluatorConditionBuilder
        => factory.Where(e =>
        {
            observe(e);
            var success = @break != true;
            return success;
        });
}
