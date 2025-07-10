using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TRMApi.Models;

namespace TRMApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {

        //public DbSet<ApplicationRole> ApplicationRoles { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
        {
        }
    }
}
