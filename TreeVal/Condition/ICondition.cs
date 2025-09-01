namespace TreeVal.Condition;

public interface ICondition<in TNode> : IDescribable
{
    void Evaluate(TNode node, Evaluation evaluation);
}
