using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskManager.DAL;

namespace TaskManager.Models
{
    public class ListTaskViewModel
    {

        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Задача")]
        public string Name { get; set; }

        [Display(Name = "Тип")]
        public string Type { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Приоритет")]
        public string Priority { get; set; }

        [Display(Name = "Назначена")]
        public string User { get; set; }

        [Display(Name = "Статус")]
        public string Status { get; set; }

        [Display(Name = "Дата создания")]
        public DateTime DateCreate { get; set; }

        [Display(Name = "Дата завершения")]
        public DateTime? DateClose { get; set; }

    }
}