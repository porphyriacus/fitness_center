using Application.Common.Behaviors.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Clients.Errors
{
    public static class ClientErrors
    {
        public static readonly Error NotFound =
            Error.NotFound("Client.NotFound", "Клиент не найден");

        public static readonly Error InvalidName =
            Error.Validation("Client.InvalidName", "Имя не может быть пустым");

        public static readonly Error InvalidSurame =
            Error.Validation("Client.InvalidName", "Фамилия не может быть пуста");
        /*
        public static readonly Error DuplicateContact =
            Error.Conflict("Client.DuplicateContact", "Клиент с таким контактом уже существует");

        public static readonly Error InvalidEmail =
            Error.Validation("Client.InvalidEmail", "Некорректный формат email");
        */
    }
}
