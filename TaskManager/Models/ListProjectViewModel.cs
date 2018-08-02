using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskManager.DAL;

namespace TaskManager.Models
{
    public class ListProjectViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Имя проекта")]
        public string ProjectName { get; set; }

        [Display(Name = "Руководитель проекта")]
        public string ProjectManager { get; set; }

        [Display(Name = "Дата создания проекта")]
        public string DateCreate { get; set; }

        [Display(Name = "Дата завершения проекта")]
        public string DateClose { get; set; }

    }
}