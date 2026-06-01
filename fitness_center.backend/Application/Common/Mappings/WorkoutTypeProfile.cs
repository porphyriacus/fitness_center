using Application.Features.WorkoutTypes;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Mappings
{
    public class WorkoutTypeProfile : Profile
    {
        public WorkoutTypeProfile()
        {
            CreateMap<WorkoutType, WorkoutTypeDto>();
        }
    }
}
