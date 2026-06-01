using Application.Features.MembershipTypes;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Mappings
{
    public class MembershipTypeProfile : Profile
    {
        public MembershipTypeProfile()
        {
            CreateMap<MembershipType, MembershipTypeDto>();
        }
    }
}
