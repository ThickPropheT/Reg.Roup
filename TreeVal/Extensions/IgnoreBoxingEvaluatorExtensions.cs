using System.Linq.Expressions;
using TreeVal.__Temp__;
using TreeVal.Condition;
using TreeVal.Media;

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
        => new _IgnoreBoxingEvaluatorBuilder(factory, hint);


    private class _IgnoreBoxingEvaluatorBuilder : IVisitorNodeFactory
    {
        private readonly IVisitorNodeFactory _source;
        private readonly BoxingEvaluationHint _hint;

        public _IgnoreBoxingEvaluatorBuilder(IVisitorNodeFactory source, BoxingEvaluationHint hint)
        {
            _source = source;
            _hint = hint;
        }

        public IEvaluatorBuilder Where(Func<Node, bool> predicate, string predicateExpression = "")
            => new ProxyEvaluatorBuilder((conditions, childLookups) =>
                IgnoreBoxing(conditions, childLookups, _source.Where(predicate, predicateExpression)));

        public IEvaluatorBuilder<T> OfType<T>()
            => new ProxyEvaluatorBuilder<T>((conditions, childLookups) =>
                IgnoreBoxing(conditions, childLookups, _source.OfType<T>()));

        public IEvaluatorConditionBuilder OneOf(
            INodeEvaluatorFactory option1, INodeEvaluatorFactory option2, params INodeEvaluatorFactory[] options)
        {
            throw new SkepticalException("This has never been actuated and probably doesn't work correctly.");

            var builder = new ProxyEvaluatorBuilder((conditions, _) =>
                new OneOfNodeEvaluator(conditions, new[]
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

        private OneOfNodeEvaluator IgnoreBoxing(
            IEnumerable<ICondition> conditions,
            IEnumerable<Func<Node, IEnumerable<INodeEvaluatorFactory>>> childLookups,
            IEvaluatorBuilder candidate
        )
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

            return new OneOfNodeEvaluator([],
                _hint == BoxingEvaluationHint.Lazy
                    ? [candidate, boxed]
                    : [boxed, candidate]);
        }
    }
}
