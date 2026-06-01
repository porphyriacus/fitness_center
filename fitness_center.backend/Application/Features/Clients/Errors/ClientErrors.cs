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

    }
}
