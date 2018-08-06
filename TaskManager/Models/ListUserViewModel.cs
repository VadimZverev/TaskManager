using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaskManager.Models
{
    public class ListUserViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "ФИО")]
        public string FIO { get; set; }

        [Display(Name = "Уровень прав")]
        public string Role { get; set; }

    }
}