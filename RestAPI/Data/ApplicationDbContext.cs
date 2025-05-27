using System.Collections.Generic;
using System.Data.Common;
using RestAPI.Data;
using Microsoft.EntityFrameworkCore;
using RestAPI.ViewModel;

namespace RestAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

    }

}
