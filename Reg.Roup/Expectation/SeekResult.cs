using System;

namespace Reg.Roup.Expectation;

public class SeekResult
{
    private readonly Action<Func<IEvaluationFrameBuilder>> onPop;

    public IEvaluationFrameBuilder? Next { get; }

    public SeekResult(IEvaluationFrameBuilder? next, Action<Func<IEvaluationFrameBuilder>>? onPop = null)
    {
        Next = next;
        this.onPop = onPop ?? (pop => pop());
    }

    public void OnPop(Func<IEvaluationFrameBuilder> pop) => onPop(pop);
}