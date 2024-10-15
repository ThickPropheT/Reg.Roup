using Reg.Roup.Conversions;
using Reg.Roup.Expression;
using Reg.Roup.Schema;
using System;
using System.Text.RegularExpressions;

namespace Reg.Roup
{
    public class MatchContext(Regex? regex, Match match)
    {
        public void Validate()
        {
            if (!match.Success)
            {
                //throw new FormatException(
                //    "Regex could not match input string."
                //);
            }
        }

        public IExpression<string?> CreateReader(SchemaMember member)
        {
            if (regex != null
                && !member.IsValid(regex))
            {
                throw new FormatException(
                    $"Regex does not contain a group definition named '{member.Name}'."
                );
            }

            return member.CreateReaderOf(match.Groups);
        }
    }
}
