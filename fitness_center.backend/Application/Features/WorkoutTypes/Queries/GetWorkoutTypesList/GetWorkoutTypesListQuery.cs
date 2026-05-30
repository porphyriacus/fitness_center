using Application.Common.Models;
using Application.Features.Trainers.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.WorkoutTypes.Queries.GetWorkoutTypesList
{

    public class GetWorkoutTypesListQuery : IRequest<Result<IReadOnlyList<WorkoutTypeDto>>>
    {
        public string? SearchField { get; set; }
        public string? SearchTerm { get; set; }

        public string? SortBy { get; set; }
        public bool SortDescending { get; set; }
    }
}
