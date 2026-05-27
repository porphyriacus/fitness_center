using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Behaviors.Errors;
using Application.Common.Models;
using FluentValidation;
using MediatR;

namespace Application.Common.Behaviors
{


    /// <summary>
    /// Автоматическая валидация команд и запросов
    /// </summary>
    /// <typeparam name="TRequest">Тип запроса</typeparam>
    /// <typeparam name="TResponse">Тип ответа (Result или Result{T})</typeparam>
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : Result
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (!_validators.Any())
                return await next();

            var failures = _validators
                .Select(v => v.Validate(request))
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();


            if (!failures.Any())
                return await next();

            var errorMessages = string.Join("; ", failures.Select(f => f.ErrorMessage));
            var error = Error.Validation(errorMessages);

            return CreateFailureResult(error);
        }

        private static TResponse CreateFailureResult(Error error)
        {
            var responseType = typeof(TResponse);

            // 1 Result<T> 
            if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(Result<>))
            {
                var valueType = responseType.GetGenericArguments()[0];
                var resultType = typeof(Result<>).MakeGenericType(valueType);
                var failureMethod = resultType.GetMethod("Failure", new[] { typeof(Error) });

                if (failureMethod is not null)
                {
                    var failureResult = failureMethod.Invoke(null, new[] { error });
                    return (TResponse)failureResult!;
                }
            }

            // 2. Result 
            if (responseType == typeof(Result))
            {
                return (TResponse)(object)Result.Failure(error);
            }

            throw new InvalidOperationException($"Тип {responseType.Name} не поддерживается. TResponse должен быть Result или Result<T>");
        }
    }
}
