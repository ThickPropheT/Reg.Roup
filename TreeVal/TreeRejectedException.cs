namespace TreeVal;

public class TreeRejectedException : Exception
{
    public TapeHead? Head { get; init; }

    private TreeRejectedException(string message)
        : base(message)
    {
    }

    private TreeRejectedException(string message, Exception inner)
        : base(message, inner)
    {
    }

    public static TreeRejectedException ForTrace()
    {
        return new TreeRejectedException("Something was rejected.");
    }

    public static TreeRejectedException ForIncompleteRead(TapeHead head)
    {
        return new TreeRejectedException("Didn't finish reading head.")
        {
            Head = head
        };
    }

    // TODO should message be passable-in here?
    public static TreeRejectedException ForError(TapeHead head, Exception error)
        => new("Something exploded unexpectedly.", error)
        {
            Head = head
        };
}
