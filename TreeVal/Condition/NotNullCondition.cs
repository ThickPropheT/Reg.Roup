using System.Linq.Expressions;

namespace TreeVal.Condition;

public class NotNullCondition : ICondition
{
    public string Describe(Expression? _)
        => "Condition.NotNull";

    public bool Evaluate(Expression? node)
        => node is not null;
}
