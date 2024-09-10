using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Reg.Roup.Conversions;
using Reg.Roup.Schema;

namespace Reg.Roup
{
    public static class RegexExtensions
    {
        public static T DeserializeMatch<T>(this Regex regex, string input, Expression<Func<IParse, T>> deserializationSchema)
            => DeserializeTo<T>(new MatchContext(regex, regex.Match(input)), DeserializationSchema.From(deserializationSchema));

        public static T Deserialize<T>(this Match match, Expression<Func<IParse, T>> deserializationSchema, Regex? regex = null)
            => DeserializeTo<T>(new MatchContext(regex, match), DeserializationSchema.From(deserializationSchema));

        private static T DeserializeTo<T>(MatchContext match, NewSchema schema)
        {
            match.Validate();

            try
            {
                return (T)schema.CreateInstanceFrom(match);
            }
            catch (NotSupportedException ex)
            {
                throw new ArgumentException("Expression format is not supported.", nameof(schema), ex);
            }
            catch { throw; }
        }
    }
}
