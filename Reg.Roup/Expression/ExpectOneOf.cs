using System.Linq;

namespace Reg.Roup.Expression
{
    using System.Linq.Expressions;

    public class ExpectOneOf : IEvaluationFrameBuilder
    {
        private readonly IEvaluationFrameBuilder[] options;

        public ExpectOneOf(IEvaluationFrameBuilder[] options)
        {
            this.options = options;
        }

        public IEvaluationFrame BuildFrame(Expression? node)
        {
            var match = options
                .Select(o => o.BuildFrame(node))
                .FirstOrDefault(f => f is not ErrorFrame);

            if (match == null)
            {
                return ErrorFrame.NotFound(this);
            }

            return EvaluationFrame
                .Found(this, e => match
                    .SeekNext(e)
                    ?.OnPush((self, controller)=>
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
}