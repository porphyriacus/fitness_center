using Application.Common.Models;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.MembershipTypes.Queries.GetMembershipTypesList
{
    internal class GetMembershipTypesListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetMembershipTypesListQuery, Result<IReadOnlyList<MembershipTypeDto>>>
    {
        public async Task<Result<IReadOnlyList<MembershipTypeDto>>> Handle(GetMembershipTypesListQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<MembershipType, bool>>? filter = null;

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                var search = request.SearchTerm.ToLower();
                filter = mt => mt.Name.ToLower().Contains(search) || mt.Description.ToLower().Contains(search);
            }

            Func<IQueryable<MembershipType>, IOrderedQueryable<MembershipType>>? orderBy = null;


            var types = await unitOfWork.MembershipTypeRepository.ListAsync(filter, orderBy, cancellationToken);

            var sorted = (request.SortBy?.ToLower()) switch
            {
                "price" => request.SortDescending
                    ? types.OrderByDescending(mt => mt.Price)
                    : types.OrderBy(mt => mt.Price),
                "validitydays" => request.SortDescending
                    ? types.OrderByDescending(mt => mt.ValidityDays)
                    : types.OrderBy(mt => mt.ValidityDays),
                "sessionscount" => request.SortDescending
                    ? types.OrderByDescending(mt => mt.SessionsCount)
                    : types.OrderBy(mt => mt.SessionsCount),
                _ => request.SortDescending
                    ? types.OrderByDescending(mt => mt.Id)
                    : types.OrderBy(mt => mt.Id)
            };
            var dtos = mapper.Map<List<MembershipTypeDto>>(sorted);

            return Result<IReadOnlyList<MembershipTypeDto>>.Ok(dtos);
        }
    }
}
