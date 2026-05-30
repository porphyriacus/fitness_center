using Application.Common.Models;
using Application.Features.Trainers.DTOs;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.WorkoutTypes.Queries.GetWorkoutTypesList
{
    internal class GetWorkoutTypesListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) 
        : IRequestHandler<GetWorkoutTypesListQuery, Result<IReadOnlyList<WorkoutTypeDto>>>
    {
        public async Task<Result<IReadOnlyList<WorkoutTypeDto>>> Handle(GetWorkoutTypesListQuery request, CancellationToken cancellation)
        {

            Expression<Func<WorkoutType, bool>>? filter = null;
            if (!String.IsNullOrEmpty(request.SearchTerm))
            {
                string search = request.SearchTerm.ToLower();
                if (!String.IsNullOrEmpty(request.SearchField))
                {
                    filter = request.SearchField.ToLower() switch
                    {
                        "name" => wt => wt.Name.ToLower().Contains(search),
                        "description" => wt => wt.Description.ToLower().Contains(search),
                        _ => null
                    };

                }
                else {
                    filter =
                           wt => wt.Name.ToLower().Contains(search)
                        || wt.Description.ToLower().Contains(search);
                }

            }


            Func<IQueryable<WorkoutType>, IOrderedQueryable<WorkoutType>>? orderBy = null;
            if (!string.IsNullOrEmpty(request.SortBy))
            {
                orderBy = request.SortBy.ToLower() switch
                {
                    "name" => request.SortDescending 
                        ? q => q.OrderByDescending(c => c.Name) 
                        : q => q.OrderBy(c => c.Name),
                    "description" => request.SortDescending 
                        ? q => q.OrderByDescending(c => c.Description) 
                        : q => q.OrderBy(c => c.Description),
                    _ => q => q.OrderBy(c => c.Id)
                };
            }
            var types = await unitOfWork.WorkoutTypeRepository.ListAsync(filter, orderBy, cancellation);

            var dtos = mapper.Map<List<WorkoutTypeDto>>(types);
            return Result<IReadOnlyList<WorkoutTypeDto>>.Ok(dtos);

        }
    }
}
