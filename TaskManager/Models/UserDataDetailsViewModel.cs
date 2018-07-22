using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaskManager.Models
{
    public class UserDataDetailsViewModel
    {

        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name ="Имя")]
        public string FirstName { get; set; }

        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Display(Name = "Отчество")]
        public string MiddleName { get; set; }

        [Display(Name = "Адрес")]
        public string Address { get; set; }

        [Display(Name = "День рождения")]
        public DateTime? Birthday { get; set; }

        [Display(Name = "Мобильный")]
        [DataType(DataType.Password)]
        public decimal? Phone { get; set; }

        [Display(Name = "Город")]
        public string City { get; set; }

        [Display(Name = "Страна")]
        public string Country { get; set; }

    }
}