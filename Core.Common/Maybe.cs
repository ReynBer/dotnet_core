using System;

namespace Core.Common
{
    public struct Maybe<T> : IEquatable<Maybe<T>>
        where T : class
    {
        private readonly T _value;

        public bool HasValue => _value != null;

        private Maybe(T value)
        {
            _value = value;
        }

        public static implicit operator Maybe<T>(T value)
            => new Maybe<T>(value);

        public static bool operator ==(Maybe<T> maybe, T value)
            => (!maybe.HasValue) ? false : maybe._value.Equals(value);

        public static bool operator !=(Maybe<T> maybe, T value)
            => !(maybe == value);

        public static bool operator ==(Maybe<T> first, Maybe<T> second)
            => first.Equals(second);

        public static bool operator !=(Maybe<T> first, Maybe<T> second)
            => !(first == second);

        public override bool Equals(object obj)
        {
            if (!(obj is Maybe<T>))
                return false;
            var other = (Maybe<T>)obj;
            return Equals(other);
        }

        public bool Equals(Maybe<T> other)
        {
            if (!HasValue && !other.HasValue)
                return true;

            if (!HasValue || !other.HasValue)
                return false;

            return _value.Equals(other._value);
        }

        public override int GetHashCode()
            => _value.GetHashCode();

        public override string ToString()
            => !HasValue ? "No value" : _value.ToString();

        public T WithDefault(T defaultValue = default(T))
            => HasValue ? _value : defaultValue;

        public Maybe<TOut> MapToMaybe<TOut>(Func<T, TOut> func)
            where TOut : class
                => HasValue ? func(_value) : null;

        public TOut? MapToStruct<TOut>(Func<T, TOut?> func)
            where TOut : struct
                => HasValue ? func(_value) : null;
    }

    public class Result<T> : Result
    {
        private readonly T _value;

        public T Value
        {
            get
            {
                if (!IsSuccess)
                    throw new InvalidOperationException();
                return _value;
            }
        }

        protected internal Result(T value, bool isSuccess, string error)
            : base(isSuccess, error)
        {
            _value = value;
        }
    }
}
