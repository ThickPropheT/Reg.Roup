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

    public string Describe(Expression? node)
        => $"Expect.OneOf: {{\n{string.Join("\n| ", _options.Select(o => o.Describe(node)))}\n}}";

    public IEvaluationFrame BuildFrame(Expression? node)
    {
        var results = _options
            .Select(o => o.BuildFrame(node))
            .ToArray();
        
        var match = results
            .FirstOrDefault(f => f is not ErrorFrame);

        if (match == null)
        {
            var errorFrame = results.FirstOrDefault(f => f is ErrorFrame);
            return errorFrame ?? ErrorFrame.NotFound(this);
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
