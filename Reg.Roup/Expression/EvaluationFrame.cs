using System;

namespace Reg.Roup.Expression
{
    using System.Linq.Expressions;

    public class EvaluationFrame : IEvaluationFrame
    {
        protected readonly Func<Expression?, IEvaluationFrame?> seekNext;
        private IEvaluationFrame.Push onPush;
        private Action<IEvaluationFrame.IStackController> onPop;

        private IEvaluationFrame? next;

        public IEvaluationFrameBuilder Origin { get; }

        protected EvaluationFrame(IEvaluationFrameBuilder origin, Func<Expression?, IEvaluationFrame?> seekNext)
        {
            Origin = origin;
            this.seekNext = seekNext;
            onPush = TryPushNext;
            onPop = PopOne;
        }

        public static EvaluationFrame Found(IEvaluationFrameBuilder origin, Func<Expression?, IEvaluationFrame?> seekNext)
            => new(origin, seekNext);

        public IEvaluationFrame OnPush(IEvaluationFrame.Push onPush)
        {
            this.onPush = onPush;
            return this;
        }

        public IEvaluationFrame OnPop(Action<IEvaluationFrame.IStackController> onPop)
        {
            this.onPop = onPop;
            return this;
        }

        public virtual IEvaluationFrame? SeekNext(Expression? node)
            => next ??= seekNext(node);

        public void PushTo(IEvaluationFrame.IStackController controller)
            => onPush(this, controller);

        public void PopFrom(IEvaluationFrame.IStackController controller)
            => onPop(controller);

        private void TryPushNext(IEvaluationFrame self, IEvaluationFrame.IStackController controller)
            => controller.TryPushFrame(self);

        private void PopOne(IEvaluationFrame.IStackController controller)
            => controller.PopFrame();
    }
}