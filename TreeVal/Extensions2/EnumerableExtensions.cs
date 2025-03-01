// TODO rework extension namespaces
namespace TreeVal.Extensions2;

public static class EnumerableExtensions
{
    public static IEnumerable<TSource> TakeWhileInclusive<TSource>(
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
}
