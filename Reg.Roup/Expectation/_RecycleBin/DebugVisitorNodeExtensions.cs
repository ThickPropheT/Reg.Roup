using System;
using System.Linq.Expressions;

namespace Reg.Roup.Expectation._RecycleBin;

public static class DebugVisitorNodeExtensions
{
    public static IVisitorNode Debug(this IVisitorNodeFactory factory, Action<Expression> observe)
        => factory
            .OfType<Expression>()
            .Where(e =>
            {
                observe(e);
                return false;
            });
}