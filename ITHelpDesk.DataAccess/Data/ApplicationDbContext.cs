using System;
using System.Collections.Generic;
using System.Text;
using ITHelpDesk.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.DataAccess.Data
{
    public partial class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        public DbSet<Users> User { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Request> Request { get; set; }
        public DbSet<RequestMakers> RequestMakers { get; set; }
        public DbSet<Workers> Workers { get; set; }
        


    }
}

    

