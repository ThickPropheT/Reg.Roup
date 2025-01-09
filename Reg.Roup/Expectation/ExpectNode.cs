using System;
using System.Linq.Expressions;
using Reg.Roup.Expectation.Common.OfType;
using Reg.Roup.Expectation.Common.OneOf;

namespace Reg.Roup.Expectation;

public class ExpectNode : IExpectNode
{
    public static IExpectation<TNode> OfType<TNode>(ExpressionType? nodeType = null)
        where TNode : Expression
        => new ExpectNode().OfType<TNode>(nodeType);

    public static IExpectation<Expression> OfType(ExpressionType nodeType)
        => new ExpectNode().OfType(nodeType);

    // TODO
    //  returning IBaseExpectation is required atm for compatibility w/ VisitorEngine,
    //  but exposes potentially undesirable methods in the context of 'OneOf'.
    //  - evaluate whether VisitorEngine can be converted to accept IEvaluationFrameBuilder
    //  - evaluate whether those methods being exposed is actually ok or useful
    public static IBaseExpectation OneOf(Func<IExpectNode, IEvaluationFrameBuilder[]> getOptions)
        => new ExpectationProxy((_, expectNode) => expectNode.OneOf(getOptions(expectNode)));

    // TODO
    //  bespoke 'OneOf' methods for common things like constants would be cool.
    //  e.g. expectNode.Constant<int>().Where(@const => @const.Value == someNumber);

    // TODO
    //  investigate if/how this can be merged into implementations of IEvaluationFrameBuilder (i.e. ExpectOneOf & ExpectEach)
    private class ExpectationProxy : IBaseExpectation
    {
        private readonly IBaseExpectation.Next<Expression> _seek;

        public ExpectationProxy(IBaseExpectation.Next<Expression> seek)
        {
            _seek = seek;
        }

        public void AddCondition(IBaseExpectation.Condition<Expression> condition)
            => throw new NotSupportedException();

        public void SetNext(IBaseExpectation.Next<Expression> seek)
            => throw new NotSupportedException();

        public TExpectation TransferTo<TExpectation>(TExpectation expectation) where TExpectation : IBaseExpectation
        {
            expectation.SetNext(_seek);
            return expectation;
        }

        public IEvaluationFrame BuildFrame(Expression? node)
            => _seek.Invoke(node!, new ExpectNode()).BuildFrame(node);
    }
}
