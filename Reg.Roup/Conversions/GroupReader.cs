using System.Text.RegularExpressions;
using Reg.Roup.Expression;
using Reg.Roup.Schema;

namespace Reg.Roup.Conversions
{
    public class GroupReader(SchemaMember member, Group group) : IExpression<string?>
    {
        public SchemaMember Member { get; } = member;

        public string? Evaluate() => group.Value;
    }
}
