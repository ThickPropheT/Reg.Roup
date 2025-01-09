using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Reg.Roup.Expectation;

public abstract class BaseExpectation : IBaseExpectation
{
    private readonly List<IConditionProxy> _conditions;
    private IBaseExpectation.Next<Expression>? _seek;

    protected BaseExpectation(IBaseExpectation.Condition<Expression> nodeTypeCondition)
    {
        _conditions = [new TestNull(nodeTypeCondition)];
    }

    public void AddCondition(IBaseExpectation.Condition<Expression> condition)
        => _conditions.Add(new ForgiveNull(condition));

    public void SetNext(IBaseExpectation.Next<Expression> seek)
        => _seek = seek;

    public TExpectation TransferTo<TExpectation>(TExpectation expectation)
        where TExpectation : IBaseExpectation
    {
        foreach (var proxy in _conditions)
        {
            expectation.AddCondition(proxy.Condition);
        }

        if (_seek != null)
        {
            expectation.SetNext(_seek);
        }

        return expectation;
    }

    public IEvaluationFrame BuildFrame(Expression? node)
    {
        var failed = _conditions.FirstOrDefault(c => !c.Evaluate(node));
        var isMatch = failed == null;

        Func<IBaseExpectation.Transformer<Expression?>?>? findTransformer = null;

        if (!isMatch)
        {
            return ErrorFrame.NotFound(this);
        }

        // TODO
        if (isMatch && false)
        {
            findTransformer = null;
        }

        return EvaluationFrame.Found(
            this,
            // TODO it would be nice to find some way to reuse an instance 'Expect'
            n => _seek?.Invoke(node!, new ExpectNode())?.BuildFrame(n)
        );
    }

    private interface IConditionProxy
    {
        IBaseExpectation.Condition<Expression> Condition { get; }

        bool Evaluate(Expression? node);
    }

    private class TestNull(IBaseExpectation.Condition<Expression> condition) : IConditionProxy
    {
        public IBaseExpectation.Condition<Expression> Condition { get; } = condition;

        public bool Evaluate(Expression? node)
            => node != null && Condition(node);
    }

    private class ForgiveNull(IBaseExpectation.Condition<Expression> condition) : IConditionProxy
    {
        public IBaseExpectation.Condition<Expression> Condition { get; } = condition;

        public bool Evaluate(Expression? node)
            => Condition(node!);
    }
}
