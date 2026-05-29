using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Trainers.Commands.Delete
{
    public sealed record DeleteTrainerCommand(int id) : IRequest<Result>;

}
