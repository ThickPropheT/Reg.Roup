using System.Linq.Expressions;

namespace Reg.Roup.Expectation.Common.OfType;

public static class ExpectNodeExtensions
{
    public static IExpectation<TNode> OfType<TNode>(this IExpectNode _, ExpressionType? nodeType = null) 
        where TNode : Expression 
        => new NodeTypeExpectation<TNode>(nodeType);
    
    public static IExpectation<Expression> OfType(this IExpectNode _, ExpressionType nodeType)
        => new NodeTypeExpectation<Expression>(nodeType);
}
