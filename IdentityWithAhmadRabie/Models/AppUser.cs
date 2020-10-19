using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityWithAhmadRabie.Models
{
    public class AppUser :IdentityUser
    {
        public string City { get; set; }
        public string Street { get; set; }
        public string Zipcode { get; set; }
    }
}
