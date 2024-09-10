using Reg.Roup.Schema;

namespace Reg.Roup.Conversions
{
    public class Conversion
    {
        public static MatchSelector Extract(SchemaMember member)
            => new(member);

        public class MatchSelector(SchemaMember member)
        {
            public GroupValueConversion From(MatchContext match)
            {
                var converter = member.FindConverter();
                var value = match.FindValue(member);

                if (converter != null)
                {
                    return GroupValueConversion.Explicit(value, converter);
                }
                else if (member.Type != typeof(string))
                {
                    return GroupValueConversion.Implicit(value);
                }
                else
                {
                    return GroupValueConversion.None(value);
                }
            }

        }
    }
}
