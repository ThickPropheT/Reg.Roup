using System.Linq;
using System.Linq.Expressions;

namespace Reg.Roup.Expectation.Common.OneOf;

// TODO reconcile the naming convention for this and others under Common namespace
public class ExpectOneOf : IEvaluationFrameBuilder
{
    private readonly IEvaluationFrameBuilder[] _options;

    public ExpectOneOf(IEvaluationFrameBuilder[] options)
    {
        _options = options;
    }

    public IEvaluationFrame BuildFrame(Expression? node)
    {
        var match = _options
            .Select(o => o.BuildFrame(node))
            .FirstOrDefault(f => f is not ErrorFrame);

        if (match == null)
        {
            return ErrorFrame.NotFound(this);
        }

        return EvaluationFrame
            .Found(this, e => match
                .SeekNext(e)
                ?.OnPush((self, controller) =>
                {
                    controller.TryPushFrame(self);
                    controller.TryPushFrame(match);
                })
                .OnPop(controller =>
                {
                    var popped = controller.PopFrame();
                    popped = controller.PopFrame();
                })
            )
            .OnPush((self, controller) =>
            {
                controller.TryPushFrame(self);
                controller.TryPushFrame(match);
            })
            .OnPop(controller =>
            {
                var popped = controller.PopFrame();
                var isThis = popped.Origin == this;
                //popped = controller.PopFrame();
            });
    }
}
