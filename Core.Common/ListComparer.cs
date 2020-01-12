using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Common
{
    public class ListComparer<T> : IEqualityComparer<IEnumerable<T>>, IEqualityComparer<IEnumerable>
        where T : IEquatable<T>
    {
        public bool Equals(IEnumerable x, IEnumerable y)
            => Equals(x.Cast<T>(), y.Cast<T>());

        public bool Equals(IEnumerable<T> x, IEnumerable<T> y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(null, x)) return false;
            if (ReferenceEquals(null, y)) return false;
            if (x.Count() != y.Count()) return false;

            var enumeratorX = x.GetEnumerator();
            var enumeratorY = y.GetEnumerator();
            while (enumeratorX.MoveNext() && enumeratorY.MoveNext())
            {
                if (enumeratorX.Current == null || enumeratorY.Current == null)
                {
                    if (enumeratorX.Current == null && enumeratorX.Current == null)
                        continue;
                    return false;
                }

                if (!enumeratorX.Current.Equals(enumeratorY.Current))
                    return false;
            }
            return true;
        }

        public int GetHashCode(IEnumerable obj)
            => GetHashCode(obj.Cast<T>());

        public int GetHashCode(IEnumerable<T> values)
            => HashCodeHelper.GetHashCode(values);
    }
}
