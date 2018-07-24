﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskManager.DAL;

namespace TaskManager.Models
{
    public class CreateTaskViewModel
    {
        [Required]
        [Display(Name = "Задача")]
        public string TaskName { get; set; }

        [Display(Name = "Тип")]
        public string Type { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Приоритет")]
        public string Priority { get; set; }

        [Display(Name = "Назначен(а)")]
        public string TaskUser { get; set; }

        [Display(Name = "Статус")]
        public int Status { get; set; }


        [HiddenInput(DisplayValue = false)]
        public DateTime DateCreate { get; set; }

        public int ProjectId { get; set; }

        public int TaskPriorityId { get; set; }
        public virtual TaskPriority TaskPriority { get; set; }

        public int TaskStatusId { get; set; }
        public virtual TaskStatus TaskStatus { get; set; }

        public int TaskTypeId { get; set; }
        public virtual TaskType TaskType { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

    }
}