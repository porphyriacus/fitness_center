using Application.Common.Models;
using Application.Features.Trainers.Errors;
using Application.Features.Workout;
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


            Expression<Func<Core.Entities.Workout, bool>> filter = w =>
                w.TrainerId == request.TrainerId &&
                (!request.IncludePast || w.StartsAt >= DateTime.UtcNow) &&
                (!request.FromDate.HasValue || w.StartsAt >= request.FromDate.Value) &&
                (!request.ToDate.HasValue || w.StartsAt <= request.ToDate.Value);


            Func<IQueryable<Core.Entities.Workout>, IOrderedQueryable<Core.Entities.Workout>> orderBy =
                q => q.OrderBy(w => w.StartsAt);

            var workouts = await unitOfWork.WorkoutRepository.ListAsync(filter, orderBy, cancellationToken);
            var dtos = mapper.Map<List<WorkoutDto>>(workouts);

            return Result<IReadOnlyList<WorkoutDto>>.Ok(dtos);
        }
    }
}
