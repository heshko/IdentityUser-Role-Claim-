using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityWithAhmadRabie.Areas.Admin.Models
{
    public class RoleViewModel
    {
        [Required]
        [Display(Name ="Role")]
        [Remote(action: "IsRoleExists",controller: "Administration")]
        public string RoleName { get; set; }
    }
}
