using System.Linq.Expressions;

namespace Treeval.Expr.New;

public static class Extensions
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

    // TODO is KeyValuePair here primitive obsession?
    public static IEnumerable<KeyValuePair<string, Expression>> GetArgsMappedByParamName(this NewExpression ne)
    {
        var parameterNames = ne.GetParameterNames().ToArray();
        return ne.Arguments.Select((arg, i) => new KeyValuePair<string, Expression>(parameterNames[i], arg));
    }
}
