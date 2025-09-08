using TreeVal.Eval;
using TreeVal.Media;

namespace TreeVal.Diagnostics;

public class TreeRejectedException : Exception
{
    public TapeHead? Head { get; init; }
    public INodeEvaluation Evaluation { get; }

    private TreeRejectedException(INodeEvaluation evaluation, string message)
        : base(message)
    {
        Evaluation = evaluation;
    }

    private TreeRejectedException(INodeEvaluation evaluation, string message, Exception inner)
        : base(message, inner)
    {
        Evaluation = evaluation;
    }

    public static TreeRejectedException ForRejection(TapeHead head, INodeEvaluation evaluation)
        => new(evaluation, "An evaluator rejected the source tree")
        {
            Head = head
        };

    public static TreeRejectedException ForIncompleteRead(TapeHead head, INodeEvaluation evaluation)
        => new(evaluation, "Tape contains unread nodes")
        {
            Head = head
        };
    
    public static TreeRejectedException ForReadPastEnd(TapeHead head, INodeEvaluation evaluation)
        => new(evaluation, "Attempted to read past end of tape")
        {
            Head = head
        };

    public static TreeRejectedException ForError(TapeHead head, INodeEvaluation evaluation, Exception error)
        => new(evaluation, "An unexpected error occurred while evaluating the source tree", error)
        {
            Head = head
        };

    public static TreeRejectedException Rethrow(
        TreeRejectedException error, IDescriptionBuilder? descriptionBuilder = null)
    {
        descriptionBuilder ??= new DefaultDescriptionBuilder();

        descriptionBuilder.EmitTreeRejection(error);

        return new TreeRejectedException(
            error.Evaluation,
            descriptionBuilder.ToString(),
            error
        )
        {
            Head = error.Head
        };
    }
}
