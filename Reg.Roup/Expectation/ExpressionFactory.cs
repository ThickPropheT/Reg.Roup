using System;
using System.Linq.Expressions;
using Reg.Roup.Conversions;

namespace Reg.Roup.Expectation;

public static class ExpressionFactory
{
    public static Expression<Func<IParse, T>> InitializingType<T>(Expression<Func<IParse, T>> initializer) =>
        initializer;
}
