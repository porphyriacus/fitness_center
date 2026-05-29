using Application.Common.Models;
using Application.Features.Trainers.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Trainers.Commands.Create
{
    public sealed record CreateTrainerCommand(string Name, string Surname, string IdentityUserId, string? ProfilePhotoUrl,
        int SpecializationId, string? Description, int ExperienceYears) : IRequest<Result<TrainerDto>>;
}
