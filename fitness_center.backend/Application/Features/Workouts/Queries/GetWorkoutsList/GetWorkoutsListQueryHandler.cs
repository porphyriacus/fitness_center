using Application.Common.Models;
using Application.Features.Trainers.DTOs;
using Application.Features.Trainers.Queries.GetTrainersList;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Application.Features.Workouts.Queries.GetWorkoutsList
{
    internal class GetWorkoutsListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetWorkoutsListQuery, Result<IReadOnlyList<WorkoutDto>>>
    {
        public async Task<Result<IReadOnlyList<WorkoutDto>>> Handle(GetWorkoutsListQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Workout, bool>>? filter = w =>
                (String.IsNullOrEmpty(request.SearchTerm) || 
                    (w.WorkoutType.Name.ToLower().Contains(request.SearchTerm))
                    || (w.WorkoutType.Description.ToLower().Contains(request.SearchTerm))
                ) &&
                (!request.WorkoutTypeId.HasValue || w.WorkoutType.Id == request.WorkoutTypeId) &&
                (!request.TrainerId.HasValue || w.TrainerId == request.TrainerId) &&
                (!request.FromDate.HasValue || w.StartsAt >= request.FromDate) &&
                (!request.ToDate.HasValue || w.StartsAt <= request.ToDate) &&
                (!request.IncludePast || w.StartsAt >= DateTime.UtcNow);

            Func<IQueryable<Workout>, IOrderedQueryable<Workout>>? orderedBy = null;
            if (!String.IsNullOrEmpty(request.SortBy))
            {
                orderedBy = request.SortBy.ToLower() switch
                {
                    "name" => request.SortDescending
                        ? q => q.OrderByDescending(c => c.WorkoutType.Name)
                        : q => q.OrderBy(c => c.WorkoutType.Name),
                    "startsat" => request.SortDescending 
                        ? q => q.OrderByDescending(w => w.StartsAt) 
                        : q => q.OrderBy(w => w.StartsAt),
                    _ => q => q.OrderBy(w => w.StartsAt)

                };
            }
            else
            {
                orderedBy = w => w.OrderBy(c => c.StartsAt);
            }

            var workouts = await unitOfWork.WorkoutRepository.ListAsync(
                filter
                , orderedBy
                , cancellationToken
                , w => w.Bookings
                , w => w.Trainer
                , w => w.WorkoutType
            );

            var dtos = mapper.Map<List<WorkoutDto>>( workouts );
            return Result<IReadOnlyList<WorkoutDto>>.Ok(dtos);
        }
    }
}
