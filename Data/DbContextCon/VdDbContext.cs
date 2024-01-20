using Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Entity;

namespace Data.DbContextCon
{
    public class VdDbContext : DbContext
    {
        public VdDbContext(DbContextOptions<VdDbContext> options) : base(options)
        {
            
        }

        //dbset

        public DbSet<Info> Infos { get; set; }
        public DbSet<Demand> Demands { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
        public DbSet<Role> Roles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DemandConfiguration());
            modelBuilder.ApplyConfiguration(new InfoConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());

            modelBuilder.Entity<UserRefreshToken>().HasKey(urt => urt.UserId);

            modelBuilder.Entity<UserRefreshToken>().HasOne(urt => urt.User).WithOne(u => u.UserRefreshToken)
                .HasForeignKey<UserRefreshToken>(urt => urt.UserId);

            modelBuilder.Entity<Role>().HasData(new List<Role>() { 
                new Role { Id = 1, Name = "admin"},
                new Role { Id = 2, Name = "employee"}});

            base.OnModelCreating(modelBuilder);

        }
    }
}
