using System.Linq.Expressions;

namespace Treeval.Condition;

public class NotNullCondition : ICondition
{
    public string Describe(Expression? _)
        => "Condition.NotNull";

    public bool Evaluate(Expression? node)
        => node is not null;
}
