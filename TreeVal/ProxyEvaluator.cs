using System.Linq.Expressions;

namespace TreeVal;

public class ProxyEvaluator : IEvaluatorNode
{
    private readonly Func<IEvaluatorNode, IVisitationContext, Expression?> _onVisit;

    public ProxyEvaluator(Func<IEvaluatorNode, IVisitationContext, Expression?> onVisit)
    {
        _onVisit = onVisit;
    }

    public void Evaluate(IVisitationContext context)
        => _onVisit(this, context);
}
