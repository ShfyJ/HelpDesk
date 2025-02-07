using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using ITHelpDesk.DataAccess.Repository.Dto;
using ITHelpDesk.Models;

namespace ITHelpDesk.DataAccess.Repository.Profiles
{
    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            CreateMap<RequestMakers, RequestMakerDto>();
            CreateMap<Users, UsersDto>();
            CreateMap<Request, RequestReadDto>();
            CreateMap<RequestDto, Request>();
        }
    }
}
