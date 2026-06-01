using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Specializations.Queries.GetSpecializationsList
{

    public sealed record GetSpecializationsQuery : IRequest<Result<IReadOnlyList<SpecializationDto>>>;
}
