using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaskManager.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Необходимо ввести логин")]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Необходимо ввести пароль")]
        [Display(Name = "Пароль")]
        [StringLength(50, ErrorMessage = "Длина пароля должна быть не менее {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Необходимо ввести пароль")]
        [Compare("Password", ErrorMessage = "Пароль и подтверждение пароля не совпадают.")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        public string PasswordConfirm { get; set; }
    }
}