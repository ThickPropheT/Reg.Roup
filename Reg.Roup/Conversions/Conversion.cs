using Reg.Roup.Expression;
using Reg.Roup.Schema;

namespace Reg.Roup.Conversions
{
    public class Conversion
    {
        public static MatchSelector Extract(SchemaMember member)
            => new(member);

        public class MatchSelector(SchemaMember member)
        {
            public IExpression<object?> From(MatchContext match)
            {
                var converter = member.FindConverter();
                var reader = match.CreateReader(member);

                if (converter != null)
                {
                    return ValueConversion.Explicit(reader, converter);
                }
                else if (member.Type != typeof(string))
                {
                    return ValueConversion.Implicit(member, reader);
                }
                else
                {
                    return ValueConversion.None(reader);
                }
            }

        }
    }
}
