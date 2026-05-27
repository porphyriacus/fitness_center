using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Behaviors.Errors
{
    public record Error
    {
        public string Code { get; }
        public string? Message { get; }
        public ErrorType Type { get; }

        private Error(string code, string? message, ErrorType errorType)
        {
            Code = code;
            Message = message;
            Type = errorType;
        }

        public static Error None => new(string.Empty, null, ErrorType.None);
        public static Error Failure(string code, string? message = null) =>
            new(code, message, ErrorType.Failure);
        public static Error NotFound(string code, string? message = null) =>
            new(code, message, ErrorType.NotFound);
        public static Error Validation(string code, string? message = null) =>
           new(code, message, ErrorType.Validation);
        public static Error Conflict(string code, string? message = null) =>
           new(code, message, ErrorType.Conflict);
        public static Error Unauthorized(string code, string? message = null) =>
           new(code, message, ErrorType.Unauthorized);

    }

}
