using Application.Common.Models;
using Application.Features.Trainers.Errors;
using Application.Features.Workouts;
using AutoMapper;
using Core.Entities;
using LinqKit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Trainers.Queries.GetTrainerSchedule
{
    internal class GetTrainerScheduleQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) 
        : IRequestHandler<GetTrainerScheduleQuery, Result<IReadOnlyList<WorkoutDto>>>
    {
        public async Task<Result<IReadOnlyList<WorkoutDto>>> Handle(GetTrainerScheduleQuery request, CancellationToken cancellationToken)
        {
            var trainer = await unitOfWork.TrainerRepository.GetByIdAsync(request.TrainerId, cancellationToken);
            if (trainer == null) {
                return TrainerErrors.NotFound;
            }

            List<Expression<Func<Core.Entities.Workout, bool>>>? filters = new List<Expression<Func<Workout, bool>>>();

            filters.Add(w => w.TrainerId == request.TrainerId);
            if (request.IncludePast)
                filters?.Add(w => w.StartsAt >= DateTime.UtcNow);
            if(request.FromDate.HasValue)
                filters?.Add(w => w.StartsAt >= request.FromDate.Value);
            if (request.ToDate.HasValue)
                filters?.Add(w => w.StartsAt <= request.ToDate.Value);


            Func<IQueryable<Core.Entities.Workout>, IOrderedQueryable<Core.Entities.Workout>> orderBy =
                q => q.OrderBy(w => w.StartsAt);

            var workouts = await unitOfWork.WorkoutRepository.ListAsync(
                orderBy
                , filters
                , cancellationToken
                , w => w.Bookings
                , w => w.WorkoutType
                , w => w.Trainer);

            var dtos = mapper.Map<List<WorkoutDto>>(workouts);

            return Result<IReadOnlyList<WorkoutDto>>.Ok(dtos);
        }
    }
}
