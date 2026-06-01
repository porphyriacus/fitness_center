using Application.Features.Memberships;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Mappings
{
    public class MembershipProfile : Profile
    {
        public MembershipProfile()
        {
            CreateMap<Membership, MembershipDto>()
                .ForMember(dest => dest.MembershipTypeName,
                    opt => opt.MapFrom(src => src.MembershipType.Name));
        }
    }
}
