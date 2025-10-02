using TreeVal.Media;
using TreeVal.Visit;

namespace TreeVal.Diagnostics;

public class TreeRejectedException : Exception
{
    public ITapeHead? Head { get; init; }
    public IVisitorContext VisitorContext { get; }

    private TreeRejectedException(IVisitorContext visitorContext, string message)
        : base(message)
    {
        VisitorContext = visitorContext;
    }

    private TreeRejectedException(IVisitorContext visitorContext, string message, Exception inner)
        : base(message, inner)
    {
        VisitorContext = visitorContext;
    }

    public static TreeRejectedException ForRejection(ITapeHead head, VisitorContext visitorContext)
        => new(visitorContext, "An evaluator rejected the source tree")
        {
            Head = head
        };

    public static TreeRejectedException ForIncompleteRead(ITapeHead head, VisitorContext visitorContext)
        => new(visitorContext, "Tape contains unread nodes")
        {
            Head = head
        };

    public static TreeRejectedException ForReadPastEnd(ITapeHead head, VisitorContext visitorContext)
        => new(visitorContext, "Attempted to read past end of tape")
        {
            Head = head
        };

    public static TreeRejectedException ForError(ITapeHead head, VisitorContext visitorContext, Exception error)
        => new(visitorContext, "An unexpected error occurred while evaluating the source tree", error)
        {
            Head = head
        };

    public static TreeRejectedException Rethrow(
        VisitationException error, IDescriptionBuilder? descriptionBuilder = null)
    {
        descriptionBuilder ??= new DefaultDescriptionBuilder();

        descriptionBuilder.EmitError(error);

        return new TreeRejectedException(
            error.VisitorContext,
            descriptionBuilder.ToString(),
            error
        )
        {
            Head = error.Head
        };
    }
}
