using TreeVal.Diagnostics;
using TreeVal.Media;
using TreeVal.Scaffolding;

namespace TreeVal.Eval;

public class DefaultNodeEvaluation : INodeEvaluation
{
    private readonly INodeEvaluatorFactory _evaluatorFactory;

    private EvaluationStatus? _status;

    public Node? Target { get; private set; }
    public INodeEvaluator? Evaluator { get; private set; }

    public EvaluationStatus Status => _status ?? EvaluationStatus.Accepted;

    public IConditionEvaluation[] ConditionEvaluations { get; private set; } = [];
    public IEnumerable<INodeEvaluation> ChildEvaluations { get; private set; } = [];

    public DefaultNodeEvaluation(INodeEvaluatorFactory evaluatorFactory)
    {
        _evaluatorFactory = evaluatorFactory;
    }

    public INodeEvaluator GetEvaluator()
        => Evaluator ??= _evaluatorFactory.ToEvaluator();

    public Node? GetTarget(VisitationContext context)
    {
        if (Target != null)
            return Target;

        var evaluator = GetEvaluator();
        var moveHead = evaluator.HeadMovementStrategy.GetStrategy(context);
        return Target = moveHead(context.Head);
    }

    public void Record(IEnumerable<IConditionEvaluation> conditionEvaluations)
    {
        var evaluations = conditionEvaluations.ToArray();

        ConditionEvaluations = evaluations;

        if (evaluations.All(e => e.Status == EvaluationStatus.Accepted))
            return;

        Reject();
    }

    public void Record(IEnumerable<INodeEvaluation> childEvaluations)
        => ChildEvaluations = childEvaluations;

    public void Reject()
    {
        System.Diagnostics.Debug.Assert(
            _status == null,
            $"Should mutable status be allowed? {Status} -> Rejected");

        _status = EvaluationStatus.Rejected;
    }

    public void Describe(IDescriptionBuilder descriptionBuilder)
    {
        descriptionBuilder.EmitBlock(() =>
        {
            if (Target != null)
            {
                descriptionBuilder.EmitTarget(Target);
            }

            if (ConditionEvaluations.Any())
            {
                descriptionBuilder.EmitEvaluations(ConditionEvaluations);
            }

            var childEvaluations = ChildEvaluations.ToArray();

            if (childEvaluations.Any())
            {
                descriptionBuilder.Emit("Children: ");
                descriptionBuilder.EmitArray(
                    childEvaluations, 
                    childEvaluation => childEvaluation.Describe(descriptionBuilder));
            }
        });
    }
}
