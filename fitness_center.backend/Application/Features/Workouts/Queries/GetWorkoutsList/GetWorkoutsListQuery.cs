using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Workouts.Queries.GetWorkoutsList
{
    public class GetWorkoutsListQuery : IRequest<Result<IReadOnlyList<WorkoutDto>>>
    {
        public int? WorkoutTypeId { get; set; }
        public int? TrainerId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool IncludePast { get; set; } = true;

        public string? SearchTerm { get; set; }
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; }
    }
}
