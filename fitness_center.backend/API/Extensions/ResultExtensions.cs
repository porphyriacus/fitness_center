using Application.Common.Behaviors.Errors;
using Application.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace API.Extensions
{
    public static class ResultExtensions
    {
        public static IActionResult ToActionResult<T>(this Result<T> result)
        {
            if (result.IsSuccess)
            {
                return new OkObjectResult(result.Value);
            }

            return result.Error.Type switch
            {
                ErrorType.NotFound => new NotFoundObjectResult(new {error = result.Error.Message}),

                ErrorType.Failure => new BadRequestObjectResult(new { error = result.Error.Message }),
                ErrorType.Validation => new BadRequestObjectResult(new { error = result.Error.Message }),

                ErrorType.Conflict => new ConflictObjectResult(new { error = result.Error.Message }),
                ErrorType.Unauthorized => new UnauthorizedObjectResult(new { error = result.Error.Message }),

                _ => new ObjectResult(new { error = "Внутренняя ошибка сервера" }) { StatusCode = 500 }

            };
        }
        public static IActionResult ToActionResult(this Result result)
        {
            if (result.IsSuccess)
            {
                return new NoContentResult();
            }

            return result.Error.Type switch
            {
                ErrorType.NotFound => new NotFoundObjectResult(new { error = result.Error.Message }),
                ErrorType.Validation => new BadRequestObjectResult(new { error = result.Error.Message }),
                ErrorType.Conflict => new ConflictObjectResult(new { error = result.Error.Message }),
                ErrorType.Unauthorized => new UnauthorizedObjectResult(new { error = result.Error.Message }),
                _ => new ObjectResult(new { error = "Внутренняя ошибка сервера" }) { StatusCode = 500 }
            };
        }
    }
}
/*
    public enum ErrorType
    {
            None,
            Failure,        // 500 ошибка на стороне сервера мб 400
            NotFound,       // 404
            Validation,     // 400 плохой запрос
            Conflict,       // 
            Unauthorized    // 401 не авторизован / 403 нет прав
        }
*/