namespace Reg.Roup.Expression
{
    using System;

    public class SeekResult
    {
        private readonly Action<Func<IExpectationEvaluator>> onPop;

        public IExpectationEvaluator? Next { get; }

        public SeekResult(IExpectationEvaluator? next, Action<Func<IExpectationEvaluator>>? onPop = null)
        {
            Next = next;
            this.onPop = onPop ?? new Action<Func<IExpectationEvaluator>>(pop => pop());
        }

        public void OnPop(Func<IExpectationEvaluator> pop) => onPop(pop);
    }
}