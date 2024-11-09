using System;

namespace Reg.Roup.Expression
{
    using System.Linq.Expressions;

    public class Expectation : BaseExpectation, IExpectation
    {
        public ExpressionType NodeType { get; }

        public Expectation(ExpressionType nodeType, IExpectationOptions options)
            : base(e => e.NodeType == nodeType, options)
        {
            NodeType = nodeType;
        }
    }
}