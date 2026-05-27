using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Behaviors.Errors
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; }

        protected Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None)
                throw new InvalidOperationException("Successful result cannot have an error");

            if (!isSuccess && error == Error.None)
                throw new InvalidOperationException("Failed result must have an error");

            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() =>
            new(true, Error.None);
        public static Result Failure(Error error) =>
            new(false, error);

        public static implicit operator Result(Error error) => Failure(error);
    }
}
