using System.Linq.Expressions;

namespace Treeval;

public class ProxyVisitor : IExpressionVisitorNode
{
    private readonly Func<IExpressionVisitorNode, IVisitationContext, Expression?> _onVisit;

    public ProxyVisitor(Func<IExpressionVisitorNode, IVisitationContext, Expression?> onVisit)
    {
        _onVisit = onVisit;
    }

    public Expression? Visit(IVisitationContext context)
        => _onVisit(this, context);
}
