using Application.Common.Models;
using Application.Features.WorkoutTypes.Commands.Create;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.WorkoutTypes.Commands.Update
{
    internal class UpdateWorkoutTypeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateWorkoutTypeCommand, Result<WorkoutTypeDto>>
    {
        public async Task<Result<WorkoutTypeDto>> Handle(UpdateWorkoutTypeCommand request, CancellationToken cancellationToken)
        {
            WorkoutType workoutType = await unitOfWork.WorkoutTypeRepository.GetByIdAsync(request.Id, cancellationToken);
            if (workoutType == null)
            {
                return WorkoutTypeErrors.NotFound;
            }

            workoutType.ChangeDescription(request.Description);
            workoutType.ChangeColor(request.Color);


            await unitOfWork.WorkoutTypeRepository.UpdateAsync(workoutType);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return mapper.Map<WorkoutTypeDto>(workoutType);
        }
    }
}
