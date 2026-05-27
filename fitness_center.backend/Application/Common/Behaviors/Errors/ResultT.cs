using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Behaviors.Errors
{
    public class Result<T> : Result
    {
        private readonly T? _value;
        private Result(T result): base(true, Error.None)
        {
            _value = result;
        }
        private Result(Error error) : base(false, error)
        {
            _value = default;
        }

        public T Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException("Value cannot be accessed when IsFailure");

        public static Result<T> Ok(T result) =>
            new (result);

        public static Result<T> Failure(Error erroe) =>
            new (erroe);

        // Неявные преобразования
        public static implicit operator Result<T>(T value) => Ok(value);
        public static implicit operator Result<T>(Error error) => Failure(error);
    }
}
