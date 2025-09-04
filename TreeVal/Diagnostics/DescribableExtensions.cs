using TreeVal.Eval.Condition;

namespace TreeVal.Diagnostics;

public static class DescribableExtensions
{
    public static void Describe(this ICondition c, IDescriptionBuilder descriptionBuilder)
        => descriptionBuilder.EmitLine($"{c},");
}
