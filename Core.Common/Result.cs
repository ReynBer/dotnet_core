using System;
using System.Linq;

namespace Core.Common
{

    public class Result
    {
        public bool IsSuccess { get; }
        public string Error { get; }

        protected Result(bool isSuccess, string error)
        {
            if (isSuccess && error != string.Empty)
                throw new InvalidOperationException();
            if (!isSuccess && error == string.Empty)
                throw new InvalidOperationException();
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Fail(string message)
            => new Result(false, message);

        public static Result<T> Fail<T>(string message)
            => new Result<T>(default(T), false, message);

        public static Result Ok()
            => new Result(true, string.Empty);

        public static Result<T> Ok<T>(T value)
            => new Result<T>(value, true, string.Empty);

        public static Result Combine(params Result[] results)
        {
            var result = results.FirstOrDefault(r => !r.IsSuccess);
            return (result != null)
                ? result
                : Ok();
        }
    }
}
