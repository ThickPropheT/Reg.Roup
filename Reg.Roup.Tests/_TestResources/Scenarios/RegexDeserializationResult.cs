namespace Reg.Roup.Tests._TestResources.Scenarios
{
    public class RegexDeserializationResult<TActual>(TActual actual)
    {
        public TActual Actual { get; } = actual;

        public void Assert<TExpected>(TExpected expected, Action<TExpected, TActual> applyAssertions)
            => applyAssertions(expected, Actual);
    }

    public class RegexDeserializationResult<TExpected, TActual>(TExpected expected, TActual actual)
        : RegexDeserializationResult<TActual>(actual)
    {
        public TExpected Expected { get; } = expected;

        public void Assert(Action<TExpected, TActual> applyAssertions)
            => Assert(Expected, applyAssertions);
    }
}
