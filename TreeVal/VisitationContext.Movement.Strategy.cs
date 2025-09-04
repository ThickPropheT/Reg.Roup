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
        public static MovementStrategy MoveForward { get; } = new(context => context.MoveForwardOrFail);
        public static MovementStrategy TryMoveForward { get; } = new(context => context.TryMoveForward);

        public static MovementStrategy From(Func<VisitationContext, TapeHead, Node?> strategy)
            => new(context => head => strategy(context, head));

        private readonly Func<VisitationContext, Func<TapeHead, Node?>> _lookupStrategy;

        private MovementStrategy(Func<VisitationContext, Func<TapeHead, Node?>> lookupStrategy)
        {
            _lookupStrategy = lookupStrategy;
        }

        public Func<TapeHead, Node?> GetStrategy(VisitationContext context)
            => _lookupStrategy(context);
    }
}
