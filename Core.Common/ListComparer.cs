using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Core.Common
{
    public class ListComparer<T> : IEqualityComparer<IEnumerable<T>>
    {
        private readonly Func<T, T, bool> _equalityFunc;

        public ListComparer(Func<T, T, bool> equalityFunc)
        {
            _equalityFunc = equalityFunc;
        }

        public ListComparer(IEqualityComparer<T> comparer)
        {
            _equalityFunc = comparer.Equals;
        }

        public ListComparer()
        {
            _equalityFunc = EqualityComparer<T>.Default.Equals;
        }

        public bool Equals([AllowNull] IEnumerable<T> x, [AllowNull] IEnumerable<T> y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(null, y) || ReferenceEquals(null, x)) return false;
            var xEnumerator = x.GetEnumerator();
            var yEnumerator = y.GetEnumerator();
            while (xEnumerator.MoveNext())
            {
                if (!yEnumerator.MoveNext() || !_equalityFunc(xEnumerator.Current, yEnumerator.Current))
                    return false;
            }
            return !yEnumerator.MoveNext();
        }

        public int GetHashCode([DisallowNull] IEnumerable<T> obj)
            => 0;
    }
}
