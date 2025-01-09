namespace Reg.Roup.Expectation.Common.OneOf;

public static class ExpectNodeExtensions
{
    public static IEvaluationFrameBuilder OneOf(this IExpectNode _, params IEvaluationFrameBuilder[] options)
        => new ExpectOneOf(options);
}
