using IdentityWithAhmadRabie.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityWithAhmadRabie.Models
{
    public class InputModel
    {
        [Required]
        [EmailAddress]
        [ModelNameValidation("heshko.com",ErrorMessage ="You Should user heshko.com")]
        [Remote(action: "EmailExist", controller:"Account")]
        public string Email { get; set; }

        [Required]
        [Display(Name ="City")]
        public string City { get; set; }
        [Required]
        [Display(Name = "Street")]
        public string Street { get; set; }

        [Required]
        [Display(Name = "ZipCode")]
        public string ZibCode { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6,ErrorMessage ="minst 6")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
