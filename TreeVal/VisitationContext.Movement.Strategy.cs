using TreeVal.Condition;
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

        // TODO
        //  this doesn't technically even need to have TapeHead passed in.
        //  it can be accessed via VisitationContext. dunno if that's
        //  a good idea or a bad idea :shrug:
        private readonly Func<VisitationContext, Func<TapeHead, Node?>> _lookupStrategy;

        private MovementStrategy(Func<VisitationContext, Func<TapeHead, Node?>> lookupStrategy)
        {
            _lookupStrategy = lookupStrategy;
        }

        // TODO
        //  consider doing away with the indirection here and just give the strategy
        //  it's own "Do Strategy" method. i think i had it this way originally, b/c
        //  the Evaluate* methods above were responsible for accepting/rejecting things
        //  themselves, rather than having the ValidationContext be responsible for it.
        public Func<TapeHead, Node?> GetStrategy(VisitationContext context)
            => _lookupStrategy(context);
    }
}
