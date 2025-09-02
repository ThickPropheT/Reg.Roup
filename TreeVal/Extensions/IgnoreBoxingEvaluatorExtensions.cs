using System.Linq.Expressions;
using TreeVal.Condition;

namespace TreeVal.Extensions;

public enum BoxingEvaluationHint
{
    Lazy = 0,
    Eager
}

// TODO test all the not implemented and commented out stuff in here
public static class IgnoreBoxingEvaluatorExtensions
{
    public static IVisitorNodeFactory IgnoreBoxing(
        this IVisitorNodeFactory factory, BoxingEvaluationHint hint = BoxingEvaluationHint.Lazy)
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

        public IEvaluatorBuilder Where(Func<Node, bool> predicate, string predicateExpression = "")
            => new ProxyEvaluatorBuilder((conditions, childLookups) =>
                IgnoreBoxing(conditions, childLookups, _source.Where(predicate, predicateExpression)));

        public IEvaluatorBuilder<T> OfType<T>()
        {
            throw new NotImplementedException();
        }

        // TODO
        //  these were moved to extensions class.
        //  how can i override that implementation here? 
        // public IEvaluatorBuilder OfType(ExpressionType nodeType)
        //     => new ProxyEvaluatorBuilder((conditions, childLookups) =>
        //         IgnoreBoxing(conditions, childLookups, _source.OfType(nodeType)));
        //
        // public IEvaluatorBuilder<TExpression> OfType<TExpression>(ExpressionType? nodeType = null)
        //     where TExpression : Expression
        //     => new ProxyEvaluatorBuilder<TExpression>((conditions, childLookups) =>
        //         IgnoreBoxing(conditions, childLookups, _source.OfType<TExpression>(nodeType)));

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

        private OneOfEvaluatorNode IgnoreBoxing(IEnumerable<ICondition> conditions,
            IEnumerable<Func<Node, IEnumerable<IEvaluatorNodeFactory>>> childLookups, IEvaluatorBuilder candidate)
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
