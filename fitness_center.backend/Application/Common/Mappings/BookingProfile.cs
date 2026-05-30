using Application.Features.Bookings;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Mappings
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<Booking, BookingDto>()
                .ForMember(dest => dest.ClientName,
                    opt => opt.MapFrom(src => src.Client != null
                        ? $"{src.Client.Name} {src.Client.Surname}"
                        : string.Empty))
                .ForMember(dest => dest.WorkoutName,
                    opt => opt.MapFrom(src => src.Workout != null && src.Workout.WorkoutType != null
                        ? src.Workout.WorkoutType.Name
                        : string.Empty))
                .ForMember(dest => dest.StartsAt,
                    opt => opt.MapFrom(src => src.Workout != null
                        ? src.Workout.StartsAt
                        : DateTime.MinValue))
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status.ToString()));
        }
    }
}
