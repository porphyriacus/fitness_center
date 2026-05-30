using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.WorkoutTypes.Commands.Update
{
    /// <summary>
    /// можно поменять только описание и цвет. не нравиться другие поля -- удаляйте и создавайте заново
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Description"></param>
    /// <param name="Color"></param>
    public sealed record UpdateWorkoutTypeCommand(int Id, string Description, string? Color)
    : IRequest<Result<WorkoutTypeDto>>;
}
