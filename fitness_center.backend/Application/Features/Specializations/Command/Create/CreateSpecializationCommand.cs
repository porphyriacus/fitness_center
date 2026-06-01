using Application.Common.Models;
using Application.Features.Specializations;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Specializations.Commands.Create;

public sealed record CreateSpecializationCommand(string Name) : IRequest<Result<SpecializationDto>>;