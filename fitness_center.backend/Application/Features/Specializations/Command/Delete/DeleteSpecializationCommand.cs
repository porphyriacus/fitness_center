using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Specializations.Commands.DeleteSpecialization
{
    public sealed record DeleteSpecializationCommand(int id) : IRequest<Result>;
}