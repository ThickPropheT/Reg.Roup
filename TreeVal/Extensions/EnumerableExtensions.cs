namespace TreeVal.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<TSource> TakeDoWhile<TSource>(
        this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        foreach (var element in source)
        {
            yield return element;
            
            if (!predicate(element))
            {
                break;
            }
        }
    }

    public static IEnumerable<TSource> TakeUntil<TSource>(
        this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        foreach (var element in source)
        {
            yield return element;
            
            if (predicate(element))
            {
                break;
            }
        }
    }
}
