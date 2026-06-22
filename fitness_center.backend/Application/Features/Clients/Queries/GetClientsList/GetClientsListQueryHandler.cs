using Application.Common.Models;
using Application.Features.Clients.DTOs;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Clients.Queries.GetClientsList
{
    public class GetClientsListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetClientsListQuery, Result<PagedResult<ClientDto>>>
    {
        public async Task<Result<PagedResult<ClientDto>>> Handle(GetClientsListQuery request, CancellationToken cancellationToken)
        {
            List<Expression<Func<Client, bool>>>? filter = new List<Expression<Func<Client, bool>>>(); ;
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var term = request.SearchTerm;
                filter.Add(c => c.Name.Contains(term) ||
                              c.Surname.Contains(term));
            }

            Func<IQueryable<Client>, IOrderedQueryable<Client>>? orderBy = null;
            if (!string.IsNullOrWhiteSpace(request.SortBy))
            {
                orderBy = request.SortBy.ToLower() switch
                {
                    "name" => request.SortDescending
                        ? q => q.OrderByDescending(c => c.Name)
                        : q => q.OrderBy(c => c.Name),
                    "surname" => request.SortDescending
                        ? q => q.OrderByDescending(c => c.Surname)
                        : q => q.OrderBy(c => c.Surname),
                    "registeredat" => request.SortDescending
                        ? q => q.OrderByDescending(c => c.RegisteredAt)
                        : q => q.OrderBy(c => c.RegisteredAt),
                    _ => q => q.OrderBy(c => c.Id)
                };
            }

            var (clients, totalCount) = await unitOfWork.ClientRepository.GetPagedAsync(
                request.PageNumber
                , request.PageSize
                , orderBy
                , filter
                , null
                , cancellationToken);

            var dtos = mapper.Map<List<ClientDto>>(clients);
            var result = new PagedResult<ClientDto>
            {
                  Items = dtos
                , TotalCount = totalCount
                , PageNumber = request.PageNumber
                , PageSize = request.PageSize
            };


            return Result<PagedResult<ClientDto>>.Ok(result);
        }
    }
}
