using Application.Common.Models;
using Application.Features.Clients.DTOs;
using Application.Features.Trainers.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Trainers.Queries.GetTrainerById
{
    public sealed record GetTrainerByIdQuery(int id) : IRequest<Result<TrainerDto>>;
}
