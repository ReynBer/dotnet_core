using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Core.Common.Extension
{
    public static class ComparableExtension
    {
        public static T Clamp<T>([DisallowNull] this T value, [DisallowNull] T v1, [DisallowNull] T v2)
            where T : IComparable
        {
            var min = v1.CompareTo(v2) > 0 ? v2 : v1;
            var max = v1.CompareTo(v2) > 0 ? v1 : v2;
            return value.CompareTo(min) < 0 ? min : (value.CompareTo(max) > 0 ? max : value);
        }
    }
}
