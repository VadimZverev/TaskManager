using AutoMapper;
using TaskManager.DAL;
using TaskManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskManager.Utils
{
    public static class AutoMapperConfig
    {
        public static void RegisterMapping()
        {
            Mapper.Initialize(cfg =>
            {

                cfg.CreateMap<CreateProjectView, Project>()
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(f => f.Name))
                    .ForMember(dest => dest.UserId, opt => opt.MapFrom(f => f.UserId))
                    .ForMember(dest => dest.DateCreate, opt => opt.MapFrom(f => DateTime.Now));
                
            });
        }
    }
}