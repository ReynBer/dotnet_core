using System;
using System.Linq.Expressions;

namespace Core.Common
{
    public static class IdentityHelper
    {
        public static Func<TValue, TValue> Get<TValue>()
            => v => v;
        public static Expression<Func<TValue, TValue>> GetExp<TValue>()
            => v => v;
    }
}
