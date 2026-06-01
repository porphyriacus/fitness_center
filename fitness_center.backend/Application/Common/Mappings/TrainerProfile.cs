using Application.Features.Clients.DTOs;
using Application.Features.Trainers.DTOs;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Mappings
{

    public class TrainerProfile : Profile
    {
        public TrainerProfile()
        {
            CreateMap<Trainer, TrainerDto>()
                .ForMember(dest => dest.Specialization,
                           opt => opt.MapFrom(
                               src => src.Specialization != null 
                               ? src.Specialization.Name 
                               : string.Empty));
        }
    }
}
