using System;
using System.Linq.Expressions;
using Reg.Roup.Expectation._RecycleBin;

namespace Reg.Roup.Expectation;

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
