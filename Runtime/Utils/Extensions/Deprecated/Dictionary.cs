using System.Collections.Generic;
using System.Linq;

public static class DictionaryExtensions
{
    public static Dictionary<TKey, TElement> ToDictionary<TKey, TElement>(this ILookup<TKey,TElement> source)
    {
        return source.ToDictionary(key => key.Key, value => value.First());
    }

    public static ILookup<Tk, ILookup<Tk2, IEnumerable<Tv>>> SimplifyGroupBy2<Tk, Tk2, Tv>(this IEnumerable<IGrouping<Tk, IGrouping<Tk2, IEnumerable<Tv>>>> source)
    {
        var temp = source.ToLookup(
            s => s.Key,
            i => i.ToLookup(
                k => k.Key,
                s => s.SelectMany(i => i)
            )
        );

        return temp;
    }

    public static ILookup<Tk, IEnumerable<Tv>> SimplifyGroupBy<Tk, Tv>(this IEnumerable<IGrouping<Tk, Tv>> source)
    {
        var temp = source.ToLookup(
            keySelector: s => s.Key,
            elementSelector: i => i.Select(i => i)
        );
        return temp;
    }
}