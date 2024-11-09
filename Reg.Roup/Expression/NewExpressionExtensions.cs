using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Reg.Roup.Expression
{
    public static class NewExpressionExtensions
    {
        public static IEnumerable<string> GetParameterNames(this NewExpression ne)
        {
            if (ne.Members != null)
            {
                return ne.Members.Select(m => m.Name);
            }

            if (ne.Constructor != null)
            {
                return ne.Constructor.GetParameters().Select(p => p.Name ?? throw new SkepticalException());
            }

            return [];
        }
    }
}
