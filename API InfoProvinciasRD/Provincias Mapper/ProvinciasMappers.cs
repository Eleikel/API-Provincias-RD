using API_InfoProvinciasRD.Models;
using API_InfoProvinciasRD.Models.Dto;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_InfoProvinciasRD.Provincias_Mapper
{
    public class ProvinciasMappers : Profile
    {

        public ProvinciasMappers()
        {
            CreateMap<Region, RegionDto>().ReverseMap();
            CreateMap<Provincia, ProvinciaDto>().ReverseMap();
            CreateMap<Provincia, ProvinciaUpdateDto>().ReverseMap();
            CreateMap<Provincia, ProvinciaCreateDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
           

        }

    }
}
