namespace Reg.Roup.Expression
{
    using System;

    public class SeekResult
    {
        private readonly Action<Func<IEvaluationFrameBuilder>> onPop;

        public IEvaluationFrameBuilder? Next { get; }

        public SeekResult(IEvaluationFrameBuilder? next, Action<Func<IEvaluationFrameBuilder>>? onPop = null)
        {
            Next = next;
            this.onPop = onPop ?? new Action<Func<IEvaluationFrameBuilder>>(pop => pop());
        }

        public void OnPop(Func<IEvaluationFrameBuilder> pop) => onPop(pop);
    }
}