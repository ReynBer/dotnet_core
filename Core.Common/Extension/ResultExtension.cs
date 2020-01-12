using System;

namespace Core.Common.Extension
{
    public static class ResultExtension
    {
        public static Result<T> ToResult<T>(this Maybe<T> maybe, string errorMessage) where T : class
            => maybe.HasValue
                ? Result.Ok(maybe.WithDefault())
                : Result.Fail<T>(errorMessage);

        public static Maybe<T> ToMaybe<T>(this Result<T> result) where T : class
            => result.IsSuccess
                ? result.Value
                : null;

        public static T? ToNullable<T>(this Result<T> result) where T : struct
            => result.IsSuccess
                ? result.Value
                : (T?)null;

        public static Result<K> OnSuccess<T, K>(this Result<T> result, Func<T, K> func)
            => result.IsSuccess
                ? Result.Ok(func(result.Value))
                : Result.Fail<K>(result.Error);

        public static T OnFailure<T>(this Result<T> result, Func<string, T> func)
            => result.IsSuccess ? result.Value : func(result.Error);

        public static Result<T> Ensure<T>(this Result<T> result, Func<T, bool> predicate, string errorMessage)
            => (result.IsSuccess && !predicate(result.Value))
                ? Result.Fail<T>(errorMessage)
                : result;

        public static Result<T> OnSuccess<T>(this Result<T> result, Action<T> action)
            => result.IsSuccess
                ? result.Do(r => action(r.Value))
                : result;

        public static T OnBoth<T>(this Result result, Func<Result, T> func)
            => func(result);

        public static Result OnSuccess(this Result result, Action action)
            => result.IsSuccess
                ? result.Do(r => action())
                : result;
    }
}
