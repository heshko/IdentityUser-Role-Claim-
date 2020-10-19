using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityWithAhmadRabie.Areas.Admin.Models
{
    public class RoleViewModelUpdate
    {
        public string Id { get; set; }
       
        [Display(Name ="Role")]
        public string RoleName { get; set; }

        public List<string> Users { get; set; } = new List<string>();
    }
}
