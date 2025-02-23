namespace TreeVal;

public class TreeRejectedException : Exception
{
    public TreeRejectedException()
    {
    }

    public TreeRejectedException(string message) : base(message)
    {
    }

    public TreeRejectedException(string message, Exception inner) : base(message, inner)
    {
    }
}
