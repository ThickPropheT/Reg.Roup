using System;
using System.Collections.Generic;

namespace Reg.Roup.Expectation.Common.Each;

public static class ExpectNodeExtensions
{
    public static IEvaluationFrameBuilder Each<T>(this IExpectNode _, IEnumerator<T> enumerator, Func<T, IEvaluationFrameBuilder> body)
        => new ExpectEach<T>(enumerator, body);
}
