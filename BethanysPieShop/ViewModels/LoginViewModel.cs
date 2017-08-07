using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BethanysPieShop.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)] // when an input is going to be generated on the ui by razor and tag helpers, then actual input should be hidden (as password)
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
