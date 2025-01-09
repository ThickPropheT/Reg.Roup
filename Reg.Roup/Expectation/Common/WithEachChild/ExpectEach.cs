using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Reg.Roup.Expectation.Common.WithEachChild;

// TODO reconcile the naming convention for this and others under Common namespace
public class ExpectEach<T> : IEvaluationFrameBuilder
{
    private readonly IEnumerator<T> _enumerator;
    private readonly Func<T, IEvaluationFrameBuilder> _body;

    public ExpectEach(IEnumerator<T> enumerator, Func<T, IEvaluationFrameBuilder> body)
    {
        _enumerator = enumerator;
        _body = body;
    }

    public IEvaluationFrame BuildFrame(Expression? node)
    {
        if (!_enumerator.MoveNext())
        {
            return ErrorFrame.NotFound(this);
        }

        var inner = _body(_enumerator.Current);

        var result = inner.BuildFrame(node);

        return new NonCachingFrame(
                this,
                n =>
                {
                    if (!_enumerator.MoveNext())
                    {
                        return ErrorFrame.NotFound(this);
                    }

                    var inner = _body(_enumerator.Current);

                    var result = inner.BuildFrame(n);
                    return result;
                }
            )
            .OnPush((self, controller) =>
            {
                controller.TryPushFrame(self);
                controller.TryPushFrame(result);
            })
            .OnPop(controller =>
            {
                var popped = controller.PopFrame();
                //popped = controller.PopFrame();
            });
    }

    private class NonCachingFrame : EvaluationFrame
    {
        public NonCachingFrame(IEvaluationFrameBuilder origin, Func<Expression?, IEvaluationFrame?> seekNext)
            : base(origin, seekNext)
        {
        }

        public override IEvaluationFrame? SeekNext(Expression? node)
            => seekNext(node);
    }
}
