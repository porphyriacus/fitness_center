using Application.Common.Models;
using Application.Features.Trainers.DTOs;
using Application.Features.Workout;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Trainers.Queries.GetTrainerSchedule
{

    public sealed record GetTrainerScheduleQuery : IRequest<Result<IReadOnlyList<WorkoutDto>>>
    {
        public int TrainerId { get; set; }           // 
        public DateTime? FromDate { get; set; }      // начало периода 
        public DateTime? ToDate { get; set; }        // конец 
        public bool IncludePast { get; set; } = false; // включать прошедшие?
    }
}
