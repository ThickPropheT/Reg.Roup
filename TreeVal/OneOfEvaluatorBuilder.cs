namespace TreeVal;

public class OneOfEvaluatorBuilder : EvaluatorBuilderBase
{
    private readonly IEvaluatorBuilder[] _options;

    public OneOfEvaluatorBuilder(IEvaluatorBuilder[] options)
    {
        _options = options;
    }

    // TODO
    //  this doesn't handle conditions
    //  it probably shouldn't be able to have children
    public override IEvaluatorNode ToEvaluator()
        => new OneOfVisitor(_options);

    private class OneOfVisitor : IEvaluatorNode
    {
        private readonly IEvaluatorBuilder[] _options;

        public OneOfVisitor(IEvaluatorBuilder[] options)
        {
            _options = options;
        }

        public void Evaluate(IVisitationContext context)
        {
            foreach (var option in _options)
            {
                var tracker = context.Try(copy =>
                {
                    var visitor = option.ToEvaluator();
                    visitor.Evaluate(copy);
                });

                if (!tracker.HasRejection)
                {
                    context.Accept(this);
                    return;
                }
            }

            // TODO does this really need to reject AND throw?
            context.Reject(this);
            throw new TreeRejectedException("No OneOf matched expression");
        }
    }
}
