using System;
using System.Collections.Generic;
using System.Text;
using Entities.Configuration;
using Entities.Models;
using Entities.SeedDB;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class RepositoryContext : IdentityDbContext<User>
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CompanyConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
        }
        
        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }





    //public class RepositoryContext : DbContext
    //{
    //    public RepositoryContext(DbContextOptions<RepositoryContext> dbContextOptions) : base(dbContextOptions)
    //    {

    //    }

    //    protected override void OnModelCreating(ModelBuilder modelBuilder)
    //    {
    //        modelBuilder.ApplyConfiguration(new CompanyConfiguration());
    //        modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
    //    }

    //    public DbSet<Company> Companies { get; set; }
    //    public DbSet<Employee> Employees { get; set; }
    //}



}
