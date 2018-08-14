using System;
using System.ComponentModel.DataAnnotations;
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

        [Display(Name = "Назначен(а)")]
        public string User { get; set; }

        [Display(Name = "Статус")]
        public string Status { get; set; }

        [Display(Name = "Дата создания")]
        public string DateCreate { get; set; }

        [Display(Name = "Дата завершения")]
        public string DateClose { get; set; }

    }

    public class CreateTaskViewModel
    {
        [Required(ErrorMessage = "Необходимо заполнить")]
        [Display(Name = "Задача")]
        public string TaskName { get; set; }

        [Display(Name = "Тип")]
        [Required(ErrorMessage = "Необходимо выбрать")]
        public int TaskTypeId { get; set; }

        [Display(Name = "Описание")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Приоритет")]
        [Required(ErrorMessage = "Необходимо выбрать")]
        public int TaskPriorityId { get; set; }

        [Display(Name = "Назначен(а)")]
        [Required(ErrorMessage = "Необходимо выбрать")]
        public int UserId { get; set; }

        [Display(Name = "Статус")]
        [Required(ErrorMessage = "Необходимо выбрать")]
        public int TaskStatusId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public DateTime DateCreate { get; set; }

        public int ProjectId { get; set; }
    }

    public class EditTaskViewModel
    {
        [Required(ErrorMessage = "Необходимо заполнить")]
        [Display(Name = "Задача")]
        public string TaskName { get; set; }

        [Display(Name = "Тип")]
        [HiddenInput(DisplayValue = false)]
        public int TaskTypeId { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Приоритет")]
        [HiddenInput(DisplayValue = false)]
        public int TaskPriorityId { get; set; }

        [Display(Name = "Назначен(а)")]
        [HiddenInput(DisplayValue = false)]
        public int UserId { get; set; }

        [Display(Name = "Статус")]
        [HiddenInput(DisplayValue = false)]
        public int TaskStatusId { get; set; }

        [Display(Name = "Закрыть задачу?")]
        public bool TaskClose { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int ProjectId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public DateTime DateCreate { get; set; }

        [HiddenInput(DisplayValue = false)]
        public DateTime? DateClose { get; set; }
    }

    public class UserTasksViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int ProjectId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int TaskId { get; set; }

        [Display(Name = "Имя проекта")]
        public string ProjectName { get; set; }

        [Display(Name = "Задача")]
        public string TaskName { get; set; }

        [Display(Name = "Тип")]
        public string Type { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Приоритет")]
        public string Priority { get; set; }

        [Display(Name = "Статус")]
        public string Status { get; set; }

    }

}