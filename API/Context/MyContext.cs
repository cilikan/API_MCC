using API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Context
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {

        }
        //nama object
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Profiling> Profilings { get; set; }
        public DbSet<University> Universities { get; set; }
        public DbSet<AccountRole> AccountRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
        //membuat relasi
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //one to one employee x account
            modelBuilder.Entity<Employee>()
                .HasOne(acc => acc.Account)
                .WithOne(emp => emp.Employee)
                .HasForeignKey<Account>(acc => acc.NIK);

            //one to one account x profiling
            modelBuilder.Entity<Account>()
                .HasOne(pro => pro.Profiling)
                .WithOne(acc => acc.Account)
                .HasForeignKey<Profiling>(pro => pro.NIK);

            //many to one profiling x education
            modelBuilder.Entity<Education>()
                .HasMany(pro => pro.Profilings)
                .WithOne(edu => edu.Education);

            //one to many university x education
            modelBuilder.Entity<Education>()
                .HasOne(uni => uni.University)
                .WithMany(edu => edu.Educations);
            //one to many role x accountrole
            modelBuilder.Entity<AccountRole>()
                .HasOne(rol => rol.Role)
                .WithMany(acr => acr.AccountRoles);
            //one to many account x accountrole
            modelBuilder.Entity<AccountRole>()
                .HasOne(acc => acc.Account)
                .WithMany(acr => acr.AccountRoles);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
