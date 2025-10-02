namespace TreeVal.Extensions;

public static class ExceptionExtensions
{
    public static IEnumerable<Exception> Enumerate(this Exception exception)
    {
        foreach (var ex in exception.EnumerateImpl())
        {
            if (ex is AggregateException agg)
            {
                foreach (var inner in agg.Flatten().InnerExceptions)
                {
                    yield return inner;
                }
            }

            yield return ex;
        }
    }

    private static IEnumerable<Exception> EnumerateImpl(this Exception exception)
    {
        yield return exception;

        if (exception.InnerException == null)
            yield break;

        foreach (var inner in exception.InnerException.Enumerate())
        {
            yield return inner;
        }
    }
}
