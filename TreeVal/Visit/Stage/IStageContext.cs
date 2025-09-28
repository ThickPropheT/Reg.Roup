using TreeVal.Media;
using TreeVal.Visit.Behavior;

namespace TreeVal.Visit.Stage;

public interface IStageContext
{
    ITapeHead TapeHead { get; }

    IEnumerable<BehaviorContext> Visitations { get; }

    void RecordVisitation(BehaviorContext visitation);
}