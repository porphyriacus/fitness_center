using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Clients.DTOs;
using AutoMapper;

namespace Application.Common.Mappings
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<Client, ClientDto>().ForMember(
                dest => dest.Contact,
                opt => opt.MapFrom(src => src.Contact.Serialize())
            );  
        }
    }
}
