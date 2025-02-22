using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Reg.Roup.Expectation;

public interface INotNullOptions<out T> : INotNullOptions
{
    INotNullOptions Where(Func<T, bool> predicate, [CallerArgumentExpression(nameof(predicate))] string message = "");
}

public interface INotNullOptions
{
    INotNullOptions<T> OfType<T>();

    INotNullOptions OfNodeType(Func<ExpressionType> findNodeType);
    
    // INotNullOptions Or(INotNullOptions option1, INotNullOptions option2, params INotNullOptions[] options);
    INotNullOptions Or(Func<IConditions, INotNullOptions[]> buildOptions);

    INotNullOptions Where(Func<Expression, bool> predicate,
        [CallerArgumentExpression(nameof(predicate))]
        string message = "");
}

public interface IConditions : INotNullOptions
{
}

public abstract class BaseExpectation : IBaseExpectation
{
    private readonly NotNullConditionChain _conditionChain = new();
    private IBaseExpectation.Next<Expression>? _seek;

    protected BaseExpectation(Action<IConditions>? options = null)
    {
        options?.Invoke(_conditionChain);
    }

    public string Describe(Expression? node)
    {
        var conditionDescription = _conditionChain.Describe(node);

        if (_seek == null)
        {
            return conditionDescription;
        }

        // TODO
        return $"{conditionDescription} TIMMEH";
    }

    public void AppendCondition(ICondition condition)
        => _conditionChain.AppendCondition(condition);

    public void AppendCondition(IBaseExpectation.Condition<Expression> condition,
        [CallerArgumentExpression(nameof(condition))]
        string message = "")
        => _conditionChain.AppendCondition(new WhereCondition(message, n => condition(n)));

    public void SetNext(IBaseExpectation.Next<Expression> seek)
        => _seek = seek;

    public TExpectation TransferTo<TExpectation>(TExpectation expectation)
        where TExpectation : IBaseExpectation
    {
        _conditionChain.CopyTo(expectation);

        if (_seek != null)
        {
            expectation.SetNext(_seek);
        }

        return expectation;
    }

    public IEvaluationFrame BuildFrame(Expression? node)
    {
        var failedCondition = _conditionChain.Evaluate(node);
        var isMatch = failedCondition == null;

        Func<IBaseExpectation.Transformer<Expression?>?>? findTransformer = null;

        if (!isMatch)
        {
            return ErrorFrame.NotFound(this, failedCondition);
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

    private class NotNullConditionChain : ConditionChain
    {
        public NotNullConditionChain(IChainingStrategy? chainingStrategy = null)
            : base(chainingStrategy != null
                ? _ => chainingStrategy
                : self => new DefaultChainingStrategy(self, new Queue<ICondition>([new NotNullCondition()])))
        {
        }
    }

    private class TypalConditionChain<T> : NotNullConditionChain, INotNullOptions<T>
    {
        public TypalConditionChain(IChainingStrategy chainingStrategy)
            : base(chainingStrategy)
        {
            AppendCondition(new WhereCondition($"Condition.OfType {{ {typeof(T).Name} }}", node => node is T));
        }

        public INotNullOptions Where(Func<T, bool> predicate,
            [CallerArgumentExpression(nameof(predicate))]
            string message = "")
        {
            AppendCondition(new WhereCondition(message, node => node is T t && predicate(t)));
            return ChainingStrategy.MoveNext();
        }
    }

    private class ConditionChain : IConditionChain, IConditions, IDescribable
    {
        protected IChainingStrategy ChainingStrategy { get; }

        protected ConditionChain(Func<INotNullOptions, IChainingStrategy>? chainingStrategy = null)
        {
            ChainingStrategy = chainingStrategy != null
                ? chainingStrategy(this)
                : new DefaultChainingStrategy(this, new Queue<ICondition>());
        }

        public string Describe(Expression? node)
        {
            var conditionDescriptions = ChainingStrategy.CopyConditions().Select(c => c.Describe(node)).ToArray();

            return conditionDescriptions.Length == 1
                ? $"Expect.Conditions: {{ {conditionDescriptions[0]} }}"
                : $"Expect.Conditions: {{\n{string.Join("\n& ", conditionDescriptions)}\n}}";
        }
        
        public void AppendCondition(ICondition condition)
            => ChainingStrategy.AppendCondition(condition);

        public INotNullOptions<T> OfType<T>()
            => new TypalConditionChain<T>(ChainingStrategy);

        public INotNullOptions OfNodeType(Func<ExpressionType> findNodeType)
            => Where(node => node.NodeType == findNodeType());

        public INotNullOptions Or(Func<IConditions, INotNullOptions[]> buildOptions)
        {
            var chain = new ConditionChain();

            buildOptions(chain);
            AppendCondition(new OrCondition(chain.ChainingStrategy.CopyConditions().ToArray()));

            return ChainingStrategy.MoveNext();
        }

        public INotNullOptions Where(Func<Expression, bool> predicate,
            [CallerArgumentExpression(nameof(predicate))]
            string message = "")
        {
            AppendCondition(new WhereCondition(message, predicate));
            return ChainingStrategy.MoveNext();
        }
        
        public void CopyTo(IConditionChain chain)
        {
            foreach (var condition in ChainingStrategy.CopyConditions())
            {
                chain.AppendCondition(condition);
            }
        }
        
        public ICondition? Evaluate(Expression? node)
            => ChainingStrategy.CopyConditions().FirstOrDefault(c => !c.Evaluate(node));
    }

    private class DefaultChainingStrategy : IChainingStrategy
    {
        private readonly INotNullOptions _root;
        private readonly Queue<ICondition> _conditions;

        public DefaultChainingStrategy(INotNullOptions root, Queue<ICondition> conditions)
        {
            _root = root;
            _conditions = conditions;
        }

        public void AppendCondition(ICondition condition) => _conditions.Enqueue(condition);
        public INotNullOptions MoveNext() => _root;
        public Queue<ICondition> CopyConditions() => _conditions;
    }

    private interface IChainingStrategy
    {
        void AppendCondition(ICondition condition);
        Queue<ICondition> CopyConditions();
        INotNullOptions MoveNext();
    }

    private class OrCondition : ICondition
    {
        private readonly ICondition[] _options;

        public OrCondition(ICondition[] options)
            => _options = options;

        public string Describe(Expression? node)
            => $"Condition.OneOf: {{\n  {string.Join("  \n| ", _options.Select(o => o.Describe(node)))}\n}}";

        public bool Evaluate(Expression? node)
            => _options.Any(o => o.Evaluate(node));
    }
}
