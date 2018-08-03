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
                    .ForMember(dest => dest.DateCreate, opt => opt.MapFrom(f => DateTime.Now));

                cfg.CreateMap<Project, ListProjectViewModel>()
                    .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(f => f.Name))
                    .ForMember(dest => dest.ProjectManager, opt => opt.MapFrom(f => f.User.UserData.LastName + " " + f.User.UserData.FirstName + " " + f.User.UserData.MiddleName))
                    .ForMember(dest => dest.DateCreate, opt => opt.MapFrom(f => f.DateCreate.ToShortDateString()))
                    .ForMember(dest => dest.DateClose, opt => opt.MapFrom(f => f.DateClose.HasValue ? f.DateClose.Value.ToShortDateString() : (string)null));

                cfg.CreateMap<Project, EditProjectViewModel>()
                    .ForMember(dest => dest.ProjectManager, opt => opt.MapFrom(f => f.User.UserData.LastName + " " + f.User.UserData.FirstName + " " + f.User.UserData.MiddleName))
                    .ForMember(dest => dest.DateClose, opt => opt.MapFrom(f => f.DateClose ?? null));

                cfg.CreateMap<EditProjectViewModel, Project>();

                //___________________________________________________USER_MAP___________________________________________________//

                cfg.CreateMap<User, ListUserViewModel>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(f => f.UserDataId))
                    .ForMember(dest => dest.FIO, opt => opt.MapFrom(f => f.UserData.LastName + " " + f.UserData.FirstName + " " + f.UserData.MiddleName))
                    .ForMember(dest => dest.Role, opt => opt.MapFrom(f => f.Role.Name));

                cfg.CreateMap<UserData, UserDataDetailsViewModel>()
                    .ForMember(dest => dest.LastName, opt => opt.MapFrom(f => f.LastName ?? "Не указано"))
                    .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(f => f.MiddleName ?? "Не указано"))
                    .ForMember(dest => dest.Address, opt => opt.MapFrom(f => f.Address ?? "Не указано"))
                    .ForMember(dest => dest.City, opt => opt.MapFrom(f => f.City ?? "Не указано"))
                    .ForMember(dest => dest.Country, opt => opt.MapFrom(f => f.Country ?? "Не указано"));

                cfg.CreateMap<UserDataDetailsViewModel, UserData>()
                    .ForMember(dest => dest.LastName, opt => opt.MapFrom(f => f.LastName ?? "Не указано"))
                    .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(f => f.MiddleName ?? "Не указано"))
                    .ForMember(dest => dest.Address, opt => opt.MapFrom(f => f.Address ?? "Не указано"))
                    .ForMember(dest => dest.City, opt => opt.MapFrom(f => f.City ?? "Не указано"))
                    .ForMember(dest => dest.Country, opt => opt.MapFrom(f => f.Country ?? "Не указано"));

                cfg.CreateMap<CreateUserViewModel, User>();

                //___________________________________________________TASK_MAP___________________________________________________//

                cfg.CreateMap<Task, ListTaskViewModel>()
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(f => f.TaskName))
                    .ForMember(dest => dest.Type, opt => opt.MapFrom(f => f.TaskType.Name))
                    .ForMember(dest => dest.Priority, opt => opt.MapFrom(f => f.TaskPriority.Name))
                    .ForMember(dest => dest.User, opt => opt.MapFrom(f => f.User.UserData.LastName + " " + f.User.UserData.FirstName + " " + f.User.UserData.MiddleName))
                    .ForMember(dest => dest.Status, opt => opt.MapFrom(f => f.TaskStatus.Name))
                    .ForMember(dest => dest.DateClose, opt => opt.MapFrom(f => f.DateClose ?? null));

                cfg.CreateMap<Task, EditTaskViewModel>()
                    .ForMember(dest => dest.DateClose, opt => opt.MapFrom(f => f.DateClose ?? null));

                cfg.CreateMap<EditTaskViewModel, Task>();

                cfg.CreateMap<CreateTaskViewModel, Task>()
                    .ForMember(dest => dest.DateCreate, opt => opt.MapFrom(f => DateTime.Now));

            });
        }
    }
}