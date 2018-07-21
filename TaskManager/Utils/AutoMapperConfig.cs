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

                cfg.CreateMap<CreateProjectViewModel, Project>()
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(f => f.Name))
                    .ForMember(dest => dest.UserId, opt => opt.MapFrom(f => f.UserId))
                    .ForMember(dest => dest.DateCreate, opt => opt.MapFrom(f => DateTime.Now));

                cfg.CreateMap<Project, ListProjectViewModel>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(f => f.Id))
                    .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(f => f.Name))
                    .ForMember(dest => dest.ProjectManager, opt => opt.MapFrom(f => f.User.UserData.LastName + " " + f.User.UserData.FirstName + " " + f.User.UserData.MiddleName))
                    .ForMember(dest => dest.DateCreate, opt => opt.MapFrom(f => f.DateCreate))
                    .ForMember(dest => dest.DateClose, opt => opt.MapFrom(f => f.DateClose ?? null));
            });
        }
    }
}