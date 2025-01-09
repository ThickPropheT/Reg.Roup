using System;
using System.Linq.Expressions;

namespace Reg.Roup.Expectation;

// TODO unused
public class EvaluationResult
{
    private readonly Func<SeekResult>? _seekNext;
    private readonly Func<IBaseExpectation.Transformer<Expression?>?>? _findTransformer;

    public bool IsMatch { get; }

    public EvaluationResult(
        bool isMatch,
        Func<SeekResult>? seekNext = null,
        Func<IBaseExpectation.Transformer<Expression?>?>? findTransformer = null
    )
    {
        IsMatch = isMatch;
        _seekNext = seekNext;
        _findTransformer = findTransformer;
    }

    public static EvaluationResult PassWith(
        Func<SeekResult>? seekNext = null,
        Func<IBaseExpectation.Transformer<Expression?>?>? findTransformer = null
    ) => new(
        true,
        seekNext,
        findTransformer
    );

    public static EvaluationResult FailWith(Exception onSeek, Exception? onTransform = null)
        => new(
            false,
            () => throw onSeek,
            () => throw onTransform ?? onSeek
        );

    public SeekResult SeekNext()
        => _seekNext?.Invoke() ?? new SeekResult(null);

    public IBaseExpectation.Transformer<Expression?>? FindTransformer()
        => _findTransformer?.Invoke();
}