using System.Collections.Generic;
using System.Linq;
using LinqExpressions = System.Linq.Expressions;

namespace Reg.Roup.Expectation.NewExpression;

public static class NewExpressionExtensions
{
    public static IEnumerable<string> GetParameterNames(this LinqExpressions.NewExpression ne)
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

    public static IEnumerable<KeyValuePair<string, LinqExpressions.Expression>> GetMappedArguments(this LinqExpressions.NewExpression ne)
    {
        var parameterNames = ne.GetParameterNames().ToArray();
        return ne.Arguments.Select((arg, i) => new KeyValuePair<string, LinqExpressions.Expression>(parameterNames[i], arg));
    }
}
