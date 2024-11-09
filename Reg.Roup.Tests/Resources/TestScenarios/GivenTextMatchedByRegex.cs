using Reg.Roup.Conversions;
using Reg.Roup.Expression;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Reg.Roup.Tests.Resources.TestScenarios
{
    public class GivenTextMatchedByRegex(Func<string> getText, Regex regex)
    {
        protected readonly Func<string> GetText = getText;
        public Regex Regex { get; } = regex;

        public string _GetText() => GetText();

        public Schema DeserializeMatch<Schema>(Expression<Func<IParse, Schema>> schema, VisitorEngine visitor)
            => Regex.DeserializeMatch(GetText(), schema, visitor);
    }

    public class GivenTextMatchedByRegex<TExpected>(TExpected expected, Func<string> getText, Regex regex)
        : GivenTextMatchedByRegex(getText, regex)
    {
        public TExpected Expected { get; } = expected;

        public RegexDeserializationResult<TExpected, Actual> WhenMatchDeserializedVia<Actual>(Expression<Func<IParse, Actual>> schema)
            //=> new(Expected, DeserializeMatch(schema));
            => throw new NotImplementedException("Under construction - awaiting final solution to changes above ^^^");
    }
}
