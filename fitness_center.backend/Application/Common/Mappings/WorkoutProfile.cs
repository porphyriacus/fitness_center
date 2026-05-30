using Application.Features.Clients.DTOs;
using Application.Features.Workout;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Mappings
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkoutProfile : Profile
    {
        public WorkoutProfile()
        {
            CreateMap<Workout, WorkoutDto>()
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => src.WorkoutType.Name)
                )
                .ForMember(
                    dest => dest.WorkoutTypeId,
                    opt => opt.MapFrom(src => src.WorkoutType.Id)
                )
                .ForMember(
                    dest => dest.TrainerName,
                    opt => opt.MapFrom(src => src.Trainer.Name)
                )
                .ForMember(
                    dest => dest.CurrentBookingsCount,
                    opt => opt.MapFrom(src => src.GetCurrentBookingsCount())
                )
                .ForMember(
                    dest => dest.AvailableSlots,
                    opt => opt.MapFrom(src => src.GetAvailableSlots())
                )
                .ForMember(
                    dest => dest.DefaultMaxCapacity,
                    opt => opt.MapFrom(src => src.WorkoutType.DefaultMaxCapacity)
                )
                .ForMember(
                    dest => dest.Price,
                    opt => opt.MapFrom(src => src.WorkoutType.Price)
                )
                .ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status.ToString())
                );

        }
    }
}
