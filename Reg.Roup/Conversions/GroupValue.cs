using System.Text.RegularExpressions;
using Reg.Roup._Expression;
using Reg.Roup.Schema;

namespace Reg.Roup.Conversions
{
    public class GroupValue : IExpression<string?>
    {
        private readonly Group? _group;

        public SchemaMember Member { get; }

        private GroupValue(SchemaMember member, Group? group)
        {
            Member = member;
            _group = group;
        }

        public static GroupValue FromGroup(SchemaMember member, Group group)
            => new(member, group);

        public static GroupValue FromOptional(SchemaMember member)
            => new(member, null);

        public string? Evaluate() => _group?.Value;
    }
}
