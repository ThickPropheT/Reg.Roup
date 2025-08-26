using System.Linq.Expressions;

namespace TreeVal;

public partial class VisitationContext
{
    private Expression? MoveForwardOrFail(TapeHead head)
        => head.MoveForward();

    private Expression? TryMoveForward(TapeHead head)
        => head.CanMoveForward()
            ? head.MoveForward()
            : null;

    public class MovementStrategy
    {
        public static MovementStrategy MoveForward { get; } = new(context => context.MoveForwardOrFail);
        public static MovementStrategy TryMoveForward { get; } = new(context => context.TryMoveForward);

        private readonly Func<VisitationContext, Func<TapeHead, Expression?>> _lookupStrategy;

        private MovementStrategy(Func<VisitationContext, Func<TapeHead, Expression?>> lookupStrategy)
        {
            _lookupStrategy = lookupStrategy;
        }

        // TODO
        //  consider doing away with the indirection here and just give the strategy
        //  it's own "Do Strategy" method. i think i had it this way originally, b/c
        //  the Evaluate* methods above were responsible for accepting/rejecting things
        //  themselves, rather than having the ValidationContext be responsible for it.
        public Func<TapeHead, Expression?> GetStrategy(VisitationContext context)
            => _lookupStrategy(context);
    }
}
