using Reg.Roup.Expression;
using Reg.Roup.Schema;

namespace Reg.Roup.Conversions
{
    public class Constant(SchemaMember member, string? value) : IExpression<string?>
    {
        public SchemaMember Member { get; } = member;

        public string? Evaluate() => value;
    }
}
