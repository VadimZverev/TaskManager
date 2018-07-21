using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskManager.DAL;

namespace TaskManager.Models
{
    public class EditProjectViewModel
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "Имя проекта")]
        public string Name { get; set; }

        [Display(Name = "Руководитель проекта")]
        public string ProjectManager { get; set; }

        [Display(Name = "Закрыть проект? ")]
        public bool ProjectClose { get; set; }

        [HiddenInput(DisplayValue = false)]
        public DateTime DateCreate { get; set; }

        [HiddenInput(DisplayValue = false)]
        public DateTime? DateClose { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

    }
}