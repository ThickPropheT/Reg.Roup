using TreeVal.Eval;
using TreeVal.Media;

namespace TreeVal;

public partial class VisitationContext
{
    private Node MoveForwardOrFail(TapeHead head)
        => head.MoveForward();

    private Node? TryMoveForward(TapeHead head)
        => head.CanMoveForward()
            ? head.MoveForward()
            : null;

    public class MovementStrategy
    {
        public static MovementStrategy MoveForward { get; } = new((context, _) => context.MoveForwardOrFail);
        public static MovementStrategy TryMoveForward { get; } = new((context, _) => context.TryMoveForward);

        public static MovementStrategy From(Func<VisitationContext, TapeHead, INodeEvaluation, Node?> strategy)
            => new((context, evaluation) => head => strategy(context, head, evaluation));

        private readonly Func<VisitationContext, INodeEvaluation, Func<TapeHead, Node?>> _lookupStrategy;

        private MovementStrategy(Func<VisitationContext, INodeEvaluation, Func<TapeHead, Node?>> lookupStrategy)
        {
            _lookupStrategy = lookupStrategy;
        }

        public Func<TapeHead, Node?> GetStrategy(VisitationContext context, INodeEvaluation evaluation)
            => _lookupStrategy(context, evaluation);
    }
}
