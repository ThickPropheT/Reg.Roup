using Reg.Roup.Conversions;
using Reg.Roup.Schema;
using System;
using System.Text.RegularExpressions;

namespace Reg.Roup.Utility
{
    internal static class GroupCollectionExtensions
    {
        public static GroupValue FindValueFor(this GroupCollection groups, SchemaMember member)
        {
            var candidate = groups[member.Name];

            if (candidate != null
                && candidate.Success)
            {
                return GroupValue.FromGroup(member, candidate);
            }
            if (!member.IsOptional)
            {
                throw new FormatException(
                    $"Regex could not match group '{member.Name}' for input string, but group is not optional (nullable)."
                );
            }

            return GroupValue.FromOptional(member);
        }
    }
}
