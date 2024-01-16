using Data.Entity;
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

        public DbSet<User> Users { get; set; }
        public DbSet<Info> Infos { get; set; }
        public DbSet<Demand> Demands { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DemandConfiguration());
            modelBuilder.ApplyConfiguration(new InfoConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
