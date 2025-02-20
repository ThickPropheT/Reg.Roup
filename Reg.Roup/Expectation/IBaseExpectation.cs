using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Reg.Roup.Expectation;

// TODO these two interfaces may deserve their own files provided they survive for long
public interface ICondition : IDescribable
{
    bool Evaluate(Expression? node);
}

public interface IConditionChain
{
    void AppendCondition(ICondition condition);
}

public interface IBaseExpectation : IEvaluationFrameBuilder, IConditionChain
{
    // TODO consider renaming to Filter
    public delegate bool Condition<in TNode>(TNode node);

    public delegate T Selector<in TNode, out T>(TNode node);

    public delegate IEvaluationFrameBuilder Next<in TNode>(TNode node, IExpectNode expectNode);

    public delegate IEvaluationFrameBuilder NextChild<in TNode, in TChild>(TNode node, TChild child, int i,
        IExpectNode expectNode);

    // TODO consider merging w/ Selector
    public delegate T State<in TNode, out T>(TNode node);

    public delegate Expression Transformer<in TNode>(TNode node);

    void AppendCondition(Condition<Expression> condition, [CallerArgumentExpression(nameof(condition))] string message = "");
    void SetNext(Next<Expression> seek);

    TExpectation TransferTo<TExpectation>(TExpectation expectation)
        where TExpectation : IBaseExpectation;
}
