using System;
using System.Linq.Expressions;

namespace Reg.Roup.Expectation;

public class EvaluationFrame : IEvaluationFrame
{
    // TODO evaluate whether this truly needs to be protected
    protected readonly Func<Expression?, IEvaluationFrame?> seekNext;
    private IEvaluationFrame.Push _onPush;
    private Action<IEvaluationFrame.IStackController> _onPop;

    private IEvaluationFrame? _next;

    public IEvaluationFrameBuilder Origin { get; }

    protected EvaluationFrame(IEvaluationFrameBuilder origin, Func<Expression?, IEvaluationFrame?> seekNext)
    {
        Origin = origin;
        this.seekNext = seekNext;
        _onPush = TryPushNext;
        _onPop = PopOne;
    }

    public static EvaluationFrame Found(IEvaluationFrameBuilder origin, Func<Expression?, IEvaluationFrame?> seekNext)
        => new(origin, seekNext);

    public IEvaluationFrame OnPush(IEvaluationFrame.Push onPush)
    {
        _onPush = onPush;
        return this;
    }

    public IEvaluationFrame OnPop(Action<IEvaluationFrame.IStackController> onPop)
    {
        _onPop = onPop;
        return this;
    }

    public virtual IEvaluationFrame? SeekNext(Expression? node)
        => _next ??= seekNext(node);

    public void PushTo(IEvaluationFrame.IStackController controller)
        => _onPush(this, controller);

    public void PopFrom(IEvaluationFrame.IStackController controller)
        => _onPop(controller);

    private void TryPushNext(IEvaluationFrame self, IEvaluationFrame.IStackController controller)
        => controller.TryPushFrame(self);

    private void PopOne(IEvaluationFrame.IStackController controller)
        => controller.PopFrame();
}
