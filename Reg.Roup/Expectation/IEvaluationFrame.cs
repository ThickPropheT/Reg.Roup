using System;
using System.Linq.Expressions;

namespace Reg.Roup.Expectation;

public interface IEvaluationFrame
{
    public delegate void Push(IEvaluationFrame self, IStackController controller);

    IEvaluationFrameBuilder Origin { get; }

    IEvaluationFrame OnPush(Push onPush);
    IEvaluationFrame OnPop(Action<IStackController> onPop);

    IEvaluationFrame? SeekNext(Expression? node);
    void PushTo(IStackController controller);
    void PopFrom(IStackController controller);

    public interface IStackController
    {
        void TryPushFrame(IEvaluationFrame? frame);
        IEvaluationFrame? PopFrame();
    }
}