namespace Reg.Roup.Tests.Resources.TestScenarios
{
    public class RegexDeserializationResult<TActual>(TActual actual)
    {
        public TActual Actual { get; } = actual;

        public void AssertMultiple<TExpected>(TExpected expected, Action<TExpected, TActual> applyAssertions)
            => Assert.Multiple(() => applyAssertions(expected, Actual));
    }

    public class RegexDeserializationResult<TExpected, TActual>(TExpected expected, TActual actual)
        : RegexDeserializationResult<TActual>(actual)
    {
        public TExpected Expected { get; } = expected;

        public void AssertMultiple(Action<TExpected, TActual> applyAssertions)
            => AssertMultiple(Expected, applyAssertions);
    }
}
