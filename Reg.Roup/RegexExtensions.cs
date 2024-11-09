using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Reg.Roup.Conversions;
using Reg.Roup.Expression;
using Reg.Roup.Schema;

namespace Reg.Roup
{
    public static class RegexExtensions
    {
        // TODO
        //  consider renaming.
        //   ? MapGroups
        public static TSchema DeserializeMatch<TSchema>(this Regex regex, string input, Expression<Func<IParse, TSchema>> deserializationSchema, VisitorEngine visitor)
        //=> DeserializeTo<TSchema>(new MatchContext(regex, regex.Match(input)), DeserializationSchema.From(deserializationSchema));
        {
            var v = visitor.Visit(deserializationSchema);
            return default;
        }

        // TODO consider deleting.
        public static TSchema Deserialize<TSchema>(this Match match, Expression<Func<IParse, TSchema>> deserializationSchema, Regex? regex = null)
            => DeserializeTo<TSchema>(new MatchContext(regex, match), DeserializationSchema.From(deserializationSchema));

        private static T DeserializeTo<T>(MatchContext match, ConstructorInitSchema schema)
        {
            match.Validate();

            try
            {
                return (T)schema.Evaluate(match);
            }
            catch (NotSupportedException ex)
            {
                throw new ArgumentException("Expression format is not supported.", nameof(schema), ex);
            }
            catch { throw; }
        }
    }
}
