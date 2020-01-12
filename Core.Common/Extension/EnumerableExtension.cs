using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Common.Extension
{
    public static class EnumerableExtension
    {
        public static (T min, T max) MinMax<T>(this IEnumerable<T> values) where T : IComparable<T>
            => values.Select(v => (min: v, max: v)).Aggregate(
                (current, prev) =>
                    (min: prev.min.CompareTo(current.min) > 0 ? current.min : prev.min,
                      max: prev.max.CompareTo(current.max) > 0 ? prev.max : current.max
                    )
                );


        public static IEnumerable<TResult> GroupByTwo<TSource, TResult>(this IEnumerable<TSource> values, Func<TSource, TSource, TResult> projection)
        {
            var enumerator = values.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var v1 = enumerator.Current;
                if (enumerator.MoveNext())
                    yield return projection(v1, enumerator.Current);
            }
        }
    }
}
