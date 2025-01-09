using System.Collections.Generic;
using System.Linq;

namespace Reg.Roup.Expectation.NewExpression;

// TODO idk why, but this is still necessary here to get NewExpression to resolve below
using System.Linq.Expressions;

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
