using System;

namespace Core.Common.Extension
{
    public static class FunctionalExtension
	{
		public static T Do<T>(this T @this, Action<T> action)
		{
			action(@this);
			return @this;
		}

		public static TResult Map<TSource, TResult>(
		  this TSource @this,
		  Func<TSource, TResult> fn) =>
			fn(@this);

		public static T When<T>(
		  this T @this,
		  Func<bool> predicate,
		  Func<T, T> fn) =>
			  predicate()
				? fn(@this)
				: @this;
	}
}
