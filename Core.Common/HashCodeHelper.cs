using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Common
{

	public static class HashCodeHelper
    {
        public static int GetHashCode<T>(IEnumerable<T> values)
            where T : IEquatable<T>
        {
            unchecked
            {
                return values == null
                    ? 0
                    : values.Aggregate(0, (hash, x) => hash * 397 ^ (x != null ? x.GetHashCode() : 0));
            }
        }
    }
}
