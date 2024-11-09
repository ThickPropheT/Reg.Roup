namespace Reg.Roup.Expression
{
    using System;
    using System.Linq.Expressions;

    public class EvaluationResult
    {
        private readonly Func<SeekResult>? seekNext;
        private readonly Func<IBaseExpectation.Transformer<Expression?>?>? findTransformer;

        public bool IsMatch { get; }

        public EvaluationResult(
            bool isMatch,
            Func<SeekResult>? seekNext = null,
            Func<IBaseExpectation.Transformer<Expression?>?>? findTransformer = null
        )
        {
            IsMatch = isMatch;
            this.seekNext = seekNext;
            this.findTransformer = findTransformer;
        }

        public static EvaluationResult FailWith(Exception onSeek, Exception? onTransform = null)
            => new(
                false,
                () => throw onSeek,
                () => throw onTransform ?? onSeek
            );

        public SeekResult SeekNext()
            => seekNext?.Invoke() ?? new SeekResult(null);

        public IBaseExpectation.Transformer<Expression?>? FindTransformer()
            => findTransformer?.Invoke();
    }
}