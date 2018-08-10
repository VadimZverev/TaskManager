using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskManager.DAL;

namespace TaskManager.Models
{
    public class CreateUserViewModel
    {

        [Required(ErrorMessage = "Необходимо ввести логин")]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Необходимо ввести пароль")]
        [Display(Name = "Пароль")]
        [StringLength(50, ErrorMessage = "Длина пароля должна быть не менее {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "Необходим ввод имени")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Display(Name = "Отчество")]
        public string MiddleName { get; set; }

        [Display(Name = "Адрес")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Display(Name = "День рождения")]
        public string Birthday { get; set; }

        [Display(Name = "Мобильный")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Display(Name = "Город")]
        public string City { get; set; }

        [Display(Name = "Страна")]
        public string Country { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [Required(ErrorMessage = "Необходимо выбрать роль пользователю")]
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }

    }

    public class ListUserViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "ФИО")]
        public string FIO { get; set; }

        [Display(Name = "Уровень прав")]
        public string Role { get; set; }

    }

    public class UserDataDetailsViewModel
    {

        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Display(Name = "Отчество")]
        public string MiddleName { get; set; }

        [Display(Name = "Адрес")]
        public string Address { get; set; }

        [Display(Name = "День рождения")]
        public string Birthday { get; set; }

        [Display(Name = "Мобильный")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Display(Name = "Город")]
        public string City { get; set; }

        [Display(Name = "Страна")]
        public string Country { get; set; }

    }

    public class EditUserDataViewModel
    {

        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Необходим ввод имени")]
        [Display(Name = "Имя*")]
        public string FirstName { get; set; }

        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Display(Name = "Отчество")]
        public string MiddleName { get; set; }

        [Display(Name = "Адрес")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Display(Name = "День рождения")]
        public string Birthday { get; set; }

        [Display(Name = "Мобильный")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Display(Name = "Город")]
        public string City { get; set; }

        [Display(Name = "Страна")]
        public string Country { get; set; }

    }

}