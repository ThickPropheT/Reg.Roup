using System;
using System.Linq.Expressions;

namespace Reg.Roup.Expectation;

public class ErrorFrame : IEvaluationFrame
{
    public Exception Exception { get; }
    public IEvaluationFrameBuilder Origin { get; }

    public ErrorFrame(Exception exception, IEvaluationFrameBuilder origin)
    {
        Exception = exception;
        Origin = origin;
    }

    public static IEvaluationFrame NotFound(IEvaluationFrameBuilder origin)
        => new ErrorFrame(new Exception("Not Found"), origin);

    IEvaluationFrame IEvaluationFrame.OnPush(IEvaluationFrame.Push onPush) => this;
    IEvaluationFrame IEvaluationFrame.OnPop(Action<IEvaluationFrame.IStackController> onPop) => this;

    public IEvaluationFrame? SeekNext(Expression? node)
        => throw Exception;

    public void PushTo(IEvaluationFrame.IStackController controller)
        => throw Exception;

    public void PopFrom(IEvaluationFrame.IStackController controller)
        => throw Exception;
}
