using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityWithAhmadRabie.Models
{
    public class InputLogIn
    {

        [Required]
        [EmailAddress]
       
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }



        [Display(Name ="Remamber me ?")]
        public bool RememberMe { get; set; }

      
    }
}
