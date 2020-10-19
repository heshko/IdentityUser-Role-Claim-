using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityWithAhmadRabie.Areas.Admin.Models
{
    public class ListClaims
    {

        public static List<Claim> claims = new List<Claim>
        {
            new Claim("Create Role","Create Role"),
            new Claim("Edit Role","Edit Role"),
            new Claim("Rmove Role","Rmove Role"),
        };
    }


    public class UserClaim
    {
        public string ClaimType { get; set; }
        public bool IsSelected { get; set; }
    }
    public class UserClaimViewModel
    {
        public string Id { get; set; }

        public List<UserClaim> Claims { get; set; } = new List<UserClaim>();
        public bool IsSelected { get; set; }
    }


}
