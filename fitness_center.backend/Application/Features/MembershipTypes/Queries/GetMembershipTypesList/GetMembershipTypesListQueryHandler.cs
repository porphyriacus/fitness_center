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
                filter = mt => mt.Name.ToLower().Contains(search);
            }

            Func<IQueryable<MembershipType>, IOrderedQueryable<MembershipType>>? orderBy = null;

            if (!string.IsNullOrEmpty(request.SortBy))
            {
                orderBy = request.SortBy.ToLower() switch
                {
                    "name" => request.SortDescending
                        ? q => q.OrderByDescending(mt => mt.Name)
                        : q => q.OrderBy(mt => mt.Name),
                    "price" => request.SortDescending
                        ? q => q.OrderByDescending(mt => mt.Price)
                        : q => q.OrderBy(mt => mt.Price),
                    "validitydays" => request.SortDescending
                        ? q => q.OrderByDescending(mt => mt.ValidityDays)
                        : q => q.OrderBy(mt => mt.ValidityDays),
                    _ => q => q.OrderBy(mt => mt.Id)
                };
            }

            var types = await unitOfWork.MembershipTypeRepository.ListAsync(filter, orderBy, cancellationToken);
            var dtos = mapper.Map<List<MembershipTypeDto>>(types);

            return Result<IReadOnlyList<MembershipTypeDto>>.Ok(dtos);
        }
    }
}
