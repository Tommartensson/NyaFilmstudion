using AutoMapper;
using filmstudion.api.Models;
using Filmstudion.Models.Authentication;
using Filmstudion.Models.Filmstudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Filmstudion.Mapping
{
    public class MappingProfile : Profile
    {   
        public MappingProfile()
        {
            this.CreateMap<IRegisterFilmStudio, FilmStudio>().ReverseMap();
            this.CreateMap<User, AdminResults>().ReverseMap();
        }
    }
}
