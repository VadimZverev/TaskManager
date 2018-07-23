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
                //___________________________________________________PROJECT_MAP___________________________________________________//

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

                cfg.CreateMap<Project, EditProjectViewModel>()
                    .ForMember(dest => dest.ProjectManager, opt => opt.MapFrom(f => f.User.UserData.LastName + " " + f.User.UserData.FirstName + " " + f.User.UserData.MiddleName))
                    .ForMember(dest => dest.DateClose, opt => opt.MapFrom(f => f.DateClose ?? null));

                cfg.CreateMap<EditProjectViewModel, Project>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(f => f.Id))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(f => f.Name))
                    .ForMember(dest => dest.UserId, opt => opt.MapFrom(f => f.UserId))
                    .ForMember(dest => dest.DateCreate, opt => opt.MapFrom(f => f.DateCreate))
                    .ForMember(dest => dest.DateClose, opt => opt.MapFrom(f => f.DateClose));

                //___________________________________________________USER_MAP___________________________________________________//

                cfg.CreateMap<User, ListUserViewModel>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(f => f.UserDataId))
                    .ForMember(dest => dest.FIO, opt => opt.MapFrom(f => f.UserData.LastName + " " + f.UserData.FirstName + " " + f.UserData.MiddleName))
                    .ForMember(dest => dest.Role, opt => opt.MapFrom(f => f.Role.Name));

                cfg.CreateMap<UserData, UserDataDetailsViewModel>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(f => f.Id))
                    .ForMember(dest => dest.FirstName, opt => opt.MapFrom(f => f.FirstName))
                    .ForMember(dest => dest.LastName, opt => opt.MapFrom(f => f.LastName ?? "Не указано"))
                    .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(f => f.MiddleName ?? "Не указано"))
                    .ForMember(dest => dest.Address, opt => opt.MapFrom(f => f.Address ?? "Не указано"))
                    .ForMember(dest => dest.Birthday, opt => opt.MapFrom(f => f.Birthday))
                    .ForMember(dest => dest.Phone, opt => opt.MapFrom(f => f.Phone))
                    .ForMember(dest => dest.City, opt => opt.MapFrom(f => f.City ?? "Не указано"))
                    .ForMember(dest => dest.Country, opt => opt.MapFrom(f => f.Country ?? "Не указано"));

                cfg.CreateMap<UserDataDetailsViewModel, UserData> ()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(f => f.Id))
                    .ForMember(dest => dest.FirstName, opt => opt.MapFrom(f => f.FirstName))
                    .ForMember(dest => dest.LastName, opt => opt.MapFrom(f => f.LastName ?? "Не указано"))
                    .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(f => f.MiddleName ?? "Не указано"))
                    .ForMember(dest => dest.Address, opt => opt.MapFrom(f => f.Address ?? "Не указано"))
                    .ForMember(dest => dest.Birthday, opt => opt.MapFrom(f => f.Birthday))
                    .ForMember(dest => dest.Phone, opt => opt.MapFrom(f => f.Phone))
                    .ForMember(dest => dest.City, opt => opt.MapFrom(f => f.City ?? "Не указано"))
                    .ForMember(dest => dest.Country, opt => opt.MapFrom(f => f.Country ?? "Не указано"));

                cfg.CreateMap<CreateUserViewModel, User>()
                    .ForMember(dest => dest.Login, opt => opt.MapFrom(f => f.Login))
                    .ForMember(dest => dest.Password, opt => opt.MapFrom(f => f.Password));
            });
        }
    }
}