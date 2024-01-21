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

            modelBuilder.Entity<RoleUser>().HasKey(ky => new { ky.RoleId, ky.UserNumber});       
            modelBuilder.Entity<RoleUser>().HasData(new List<RoleUser>()
            {
                new RoleUser { UserNumber = 112233, RoleId = 1 },
                new RoleUser { UserNumber = 445566, RoleId = 2 }
            });

            modelBuilder.Entity<Demand>().HasData(new List<Demand>()
            {
                new Demand { DemandId = 1, DemandNumber = 111111, Description = "Talep 1", isActive = true, UserNumber = 445566},
                new Demand { DemandId = 2, DemandNumber = 222222, Description = "Talep 2", isActive = false, UserNumber = 445566}
            });

            modelBuilder.Entity<Info>().HasData(new List<Info>()
            {
                new Info { InfoId = 1, IBAN = "TR760009901234567800100001", InfoNumber = 452134, isActive = false, isDefault = true, UserNumber= 445566, Information = "Aktif Olan Banka Hesabı", InfoType = "Ana Hesap"},
                new Info { InfoId = 2, IBAN = "TR760009901234567800100002", InfoNumber = 652134, isActive = false, isDefault = false, UserNumber = 445566, Information = "Banka Hesabı 2", InfoType = "Yan Hesap"}
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
