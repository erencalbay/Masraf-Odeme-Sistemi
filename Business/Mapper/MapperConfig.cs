using AutoMapper;
using Data.Entity;
using Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Entity;

namespace Business.Mapper
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<UserRequest, User>();
            CreateMap<User, UserResponse>();

            CreateMap<DemandRequest, Demand>();
            CreateMap<Demand, DemandResponse>();

            CreateMap<InfoRequest, Info>();
            CreateMap<Info, InfoResponse>();
        }
    }
}
