namespace Reg.Roup.Expression
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public class ExpectEach<T> : IEvaluationFrameBuilder
    {
        private readonly IEnumerator<T> enumerator;
        private readonly Func<T, IEvaluationFrameBuilder> body;

        public ExpectEach(IEnumerator<T> enumerator, Func<T, IEvaluationFrameBuilder> body)
        {
            this.enumerator = enumerator;
            this.body = body;
        }

        public IEvaluationFrame BuildFrame(Expression? node)
        {
            if (!enumerator.MoveNext())
            {
                return ErrorFrame.NotFound(this);
            }

            var inner = body(enumerator.Current);

            var result = inner.BuildFrame(node);

            return new NonCachingFrame(
                this,
                n =>
                {
                    if (!enumerator.MoveNext())
                    {
                        return ErrorFrame.NotFound(this);
                    }

                    var inner = body(enumerator.Current);

                    var result =  inner.BuildFrame(n);
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
}