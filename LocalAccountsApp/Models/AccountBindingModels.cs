using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace LocalAccountsApp.Models
{
    // Models used as parameters to AccountController actions.

    public class LoginBindingModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
