using System;
using System.ComponentModel.DataAnnotations;
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

    public class CreateProjectViewModel
    {

        [Required(ErrorMessage = "Необходимо ввести имя проекта")]
        [StringLength(50)]
        [Display(Name = "Имя проекта")]
        public string Name { get; set; }

        [Display(Name = "Руководитель проекта")]
        public string ProjectManager { get; set; }

        [Required(ErrorMessage = "Необходимо выбрать руководителя проекта")]
        public int UserId { get; set; }
    }

    public class EditProjectViewModel
    {
        [Required(ErrorMessage = "Необходимо ввести имя проекта")]
        [StringLength(50)]
        [Display(Name = "Имя проекта")]
        public string Name { get; set; }

        [Display(Name = "Руководитель проекта")]
        public string ProjectManager { get; set; }

        [Display(Name = "Закрыть проект? ")]
        public bool ProjectClose { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string DateCreate { get; set; }

        [HiddenInput(DisplayValue = false)]
        public DateTime? DateClose { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

    }

}