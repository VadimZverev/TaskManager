using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskManager.DAL;

namespace TaskManager.Models
{
    public class CreateProjectViewModel
    {

        [Required]
        [StringLength(50)]
        [Display(Name = "Имя проекта")]
        public string Name { get; set; }

        [Display(Name = "Руководитель проекта")]
        public string ProjectManager { get; set; }


        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}