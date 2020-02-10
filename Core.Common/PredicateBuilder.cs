using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Core.Common
{
    /// <summary>
    /// http://www.albahari.com/nutshell/predicatebuilder.aspx
    /// </summary>
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1,
                                                            Expression<Func<T, bool>> expr2)
        {
            var exp = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.OrElse(expr1.Body, exp), expr1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
                                                             Expression<Func<T, bool>> expr2)
        {
            var exp = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.AndAlso(expr1.Body, exp), expr1.Parameters);
        }

        public static Expression<Func<T, bool>> From<T>(IQueryable<T> _, bool seed = true)
            => t => seed;
    }

    public static class PredicateBuilder<T>
    {
        public static Expression<Func<T, bool>> Equal<TResult>(Expression<Func<T, TResult>> proj, TResult status)
            => Expression.Lambda<Func<T, bool>>
                (Expression.Equal(proj.Body, Expression.Constant(status)), proj.Parameters);
        public static Expression<Func<T, bool>> Equal(T status)
            => Equal(IdentityHelper.GetExp<T>(), status);

        public static Expression<Func<T, bool>> In<TResult>(Expression<Func<T, TResult>> proj, IEnumerable<TResult> values)
            => values.Aggregate(Create(t => false), (acc, current) => acc.Or(Equal(proj, current)));

        public static Expression<Func<T, bool>> In(IEnumerable<T> values)
            => In(IdentityHelper.GetExp<T>(), values);

        public static Expression<Func<T, TResult>> Create<TResult>(Expression<Func<T, TResult>> proj)
            => proj;
    }
}
