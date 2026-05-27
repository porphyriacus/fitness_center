using Application.Common.Models;
using Application.Features.Clients.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Clients.Queries.GetClientsList
{
    public class GetClientsListQuery : IRequest<Result<IReadOnlyList<ClientDto>>>
    {
        public string? SearchTerm { get; set; }
        public string? SortBy { get; set; }      // "Name", "Surname", "RegisteredAt"
        public bool SortDescending { get; set; }
    }
}
