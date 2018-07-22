using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
    public class CreateUserViewModel
    {
        [Required]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Required]
        [Display(Name = "пароль")]
        [StringLength(50, ErrorMessage = "Длина пароля должна быть не менее {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароль и подтверждение пароля не совпадают.")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        public string PasswordConfirm { get; set; }
    }
}