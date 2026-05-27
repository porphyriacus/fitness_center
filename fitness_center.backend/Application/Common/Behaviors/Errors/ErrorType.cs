using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Behaviors.Errors
{
    public enum ErrorType
    {
        None,
        Failure,        // 500 ошибка на стороне сервера мб 400
        NotFound,       // 404
        Validation,     // 400 плохой запрос
        Conflict,       // 
        Unauthorized    // 401 не авторизован / 403 нет прав
    }
}
