using TreeVal.Diagnostics;

namespace TreeVal.Visit.Behavior.AfterEntering;

public class AfterEnteringVisitationResult : VisitationResult, IDescribable
{
    public IBehaviorContext BehaviorContext { get; }

    public object Behavior => BehaviorContext.Behavior;

    public AfterEnteringVisitationResult(IBehaviorContext behaviorContext)
    {
        BehaviorContext = behaviorContext;
    }

    public static AfterEnteringVisitationResult ForError(IBehaviorContext behaviorContext, Exception error)
        => new(behaviorContext) { Error = error };

    public void Describe(IDescriptionBuilder descriptionBuilder)
    {
        descriptionBuilder.EmitBlock(
            $"{nameof(AfterEnteringVisitationResult)} ",
            () =>
            {
                descriptionBuilder.Emit("Behavior: ");
                descriptionBuilder.EmitLine($"{Behavior},");
            });
    }
}
