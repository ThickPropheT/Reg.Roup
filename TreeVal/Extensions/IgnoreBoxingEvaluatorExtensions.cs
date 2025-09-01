using System.Linq.Expressions;
using TreeVal.Condition;

namespace TreeVal.Extensions;

public enum BoxingEvaluationHint
{
    Lazy = 0,
    Eager
}

public static class IgnoreBoxingEvaluatorExtensions
{
    // TODO
    //  modify this to take a child IVisitorNode, rather than doing this chaining thing.
    //  i think there's in the chaining logic that's returning new VisitorNode and orphaning
    //  the ignore boxing part
    public static IVisitorNodeFactory IgnoreBoxing(this IVisitorNodeFactory factory,
        BoxingEvaluationHint hint = BoxingEvaluationHint.Lazy)
        => new IgnoreBoxingEvaluatorBuilder(factory, hint);

    private class IgnoreBoxingEvaluatorBuilder : IVisitorNodeFactory
    {
        private readonly IVisitorNodeFactory _source;
        private readonly BoxingEvaluationHint _hint;

        public IgnoreBoxingEvaluatorBuilder(IVisitorNodeFactory source, BoxingEvaluationHint hint)
        {
            _source = source;
            _hint = hint;
        }

        public IEvaluatorBuilder<Expression> Where(Func<Expression, bool> predicate, string predicateExpression = "")
            => new ProxyEvaluatorBuilder((conditions, childLookups) =>
                IgnoreBoxing(conditions, childLookups, _source.Where(predicate, predicateExpression)));

        public IEvaluatorBuilder<Expression> OfType(ExpressionType nodeType)
            => new ProxyEvaluatorBuilder((conditions, childLookups) =>
                IgnoreBoxing(conditions, childLookups, _source.OfType(nodeType)));

        public IEvaluatorBuilder<TExpression> OfType<TExpression>(ExpressionType? nodeType = null)
            where TExpression : Expression
            => new ProxyEvaluatorBuilder<TExpression>((conditions, childLookups) =>
                IgnoreBoxing(conditions, childLookups, _source.OfType<TExpression>(nodeType)));

        public IEvaluatorConditionBuilder OneOf(IEvaluatorNodeFactory option1, IEvaluatorNodeFactory option2,
            params IEvaluatorNodeFactory[] options)
        {
            throw new SkepticalException("This has never been actuated and probably doesn't work correctly.");

            var builder = new ProxyEvaluatorBuilder((conditions, _) =>
                new OneOfEvaluatorNode(conditions, new[]
                {
                    option1,
                    _source
                        .OfType(ExpressionType.Convert)
                        .HavingChild(option1),
                    option2,
                    _source
                        .OfType(ExpressionType.Convert)
                        .HavingChild(option2)
                }.Concat(options.SelectMany(o => new[]
                    {
                        o,
                        _source
                            .OfType(ExpressionType.Convert)
                            .HavingChild(o)
                    })
                    .ToArray()).ToArray()));

            return builder;
        }

        private OneOfEvaluatorNode IgnoreBoxing<TExpression>(
            IEnumerable<ICondition<Expression>> conditions,
            IEnumerable<Func<TExpression, IEnumerable<IEvaluatorNodeFactory>>> childLookups,
            IEvaluatorBuilder<TExpression> candidate)
            where TExpression : Expression
        {
            foreach (var condition in conditions)
            {
                candidate.AddCondition(condition);
            }

            foreach (var childLookup in childLookups)
            {
                candidate.AddChildren(childLookup);
            }

            var boxed = _source
                .OfType<UnaryExpression>(ExpressionType.Convert)
                .HavingChild(candidate);

            return new OneOfEvaluatorNode([],
                _hint == BoxingEvaluationHint.Lazy
                    ? [candidate, boxed]
                    : [boxed, candidate]);
        }
    }
}
