using System.Linq.Expressions;

namespace TreeVal.Tests.__Resources.Tools;

public class ExpressionData
{
    public Expression Value { get; }
    public string? Name { get; private set; }

    private ExpressionData(Expression value)
    {
        Value = value;
    }

    public static ExpressionData FromBody(Expression<Action> expression)
        => new(expression.Body);

    public static ExpressionData FromBody<TResult>(Expression<Func<TResult>> expression)
        => new(expression.Body);

    public static implicit operator Expression(ExpressionData data)
        => data.Value;

    public ExpressionData WithName(string name)
    {
        Name = name;
        return this;
    }

    public override string ToString()
        => Name ?? Value.ToString();
}
