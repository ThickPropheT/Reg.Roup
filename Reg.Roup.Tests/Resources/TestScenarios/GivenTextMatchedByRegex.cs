using Reg.Roup.Conversions;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Reg.Roup.Tests.Resources.TestScenarios
{
    public class GivenTextMatchedByRegex(Func<string> getText, Regex regex)
    {
        protected readonly Func<string> GetText = getText;
        public Regex Regex { get; } = regex;

        public Schema DeserializeMatch<Schema>(Expression<Func<IParse, Schema>> schema)
            => Regex.DeserializeMatch(GetText(), schema);
    }

    public class GivenTextMatchedByRegex<TExpected>(TExpected expected, Func<string> getText, Regex regex)
        : GivenTextMatchedByRegex(getText, regex)
    {
        public TExpected Expected { get; } = expected;

        public RegexDeserializationResult<TExpected, Actual> WhenMatchDeserializedVia<Actual>(Expression<Func<IParse, Actual>> schema)
            => new(Expected, DeserializeMatch(schema));
    }
}
