using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;  
using AutoMapper;
using MediatR;
using Application.Common.Models;
using Application.Features.Workouts;
using Core.Abstractions;
using Core.Entities;

namespace Application.Features.Workouts.Queries.GetWorkoutsList
{
    internal class GetWorkoutsListQueryHandler : IRequestHandler<GetWorkoutsListQuery, Result<IReadOnlyList<WorkoutDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetWorkoutsListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IReadOnlyList<WorkoutDto>>> Handle(GetWorkoutsListQuery request, CancellationToken cancellationToken)
        {
            var filters = new List<Expression<Func<Workout, bool>>>();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var term = request.SearchTerm.ToLower();
                filters.Add(w =>
                    w.WorkoutType.Name.ToLower().Contains(term) ||
                    (w.WorkoutType.Description != null && w.WorkoutType.Description.ToLower().Contains(term)));
            }

            if (request.WorkoutTypeId.HasValue)
                filters.Add(w => w.WorkoutType.Id == request.WorkoutTypeId.Value);

            if (request.TrainerId.HasValue)
                filters.Add(w => w.TrainerId == request.TrainerId.Value);

            if (request.FromDate.HasValue)
                filters.Add(w => w.StartsAt >= request.FromDate.Value);

            if (request.ToDate.HasValue)
                filters.Add(w => w.StartsAt <= request.ToDate.Value);

            if (!request.IncludePast)
                filters.Add(w => w.StartsAt >= DateTime.UtcNow);

            Func<IQueryable<Workout>, IOrderedQueryable<Workout>>? orderBy = null;
            if (!string.IsNullOrEmpty(request.SortBy))
            {
                orderBy = request.SortBy.ToLower() switch
                {
                    "name" => request.SortDescending
                        ? q => q.OrderByDescending(w => w.WorkoutType.Name)
                        : q => q.OrderBy(w => w.WorkoutType.Name),
                    "startsat" => request.SortDescending
                        ? q => q.OrderByDescending(w => w.StartsAt)
                        : q => q.OrderBy(w => w.StartsAt),
                    _ => q => q.OrderBy(w => w.StartsAt)
                };
            }
            else
            {
                orderBy = q => q.OrderBy(w => w.StartsAt);
            }

            var workouts = await _unitOfWork.WorkoutRepository.ListWithFiltersAsync(
                filters,
                orderBy,
                cancellationToken,
                w => w.Bookings,
                w => w.Trainer,
                w => w.WorkoutType
            );

            var dtos = _mapper.Map<List<WorkoutDto>>(workouts);
            return Result<IReadOnlyList<WorkoutDto>>.Ok(dtos);
        }
    }
}
