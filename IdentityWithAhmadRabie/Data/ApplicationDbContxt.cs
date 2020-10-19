using IdentityWithAhmadRabie.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityWithAhmadRabie.Data
{
    public class ApplicationDbContxt : IdentityDbContext<AppUser>
    {

        public ApplicationDbContxt(DbContextOptions<ApplicationDbContxt> options) 
        : base(options)
        {

        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
