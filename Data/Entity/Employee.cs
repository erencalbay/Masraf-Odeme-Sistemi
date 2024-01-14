using Data.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WebAPI.Entity;
using Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Entity
{
    [Table("Employee", Schema = "dbo")]
    public class Employee : BaseEntity
    {
        public string IdentityNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int EmployeeNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime LastActivityDate { get; set; }

        public virtual List<Info> Infos { get; set; }
        public virtual List<Demand> Demands { get; set; }
        
    }
}
public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.Property(x => x.InsertDate).IsRequired(true);
        builder.Property(x => x.InsertUserId).IsRequired(true);
        builder.Property(x => x.UpdateUserId).IsRequired(false);
        builder.Property(x => x.UpdateDate).IsRequired(false);
        builder.Property(x => x.isActive).IsRequired(true).HasDefaultValue(true);

        builder.Property(x => x.IdentityNumber).IsRequired(true);
        builder.Property(x => x.FirstName).IsRequired(true).HasMaxLength(30);
        builder.Property(x => x.LastName).IsRequired(true).HasMaxLength(30);
        builder.Property(x => x.Email).IsRequired(true).HasDefaultValue(false);
        builder.Property(x => x.EmployeeNumber).IsRequired(true);
        builder.Property(x => x.DateOfBirth).IsRequired(true);
        builder.Property(x => x.LastActivityDate).IsRequired(true);

        builder.HasIndex(x => x.IdentityNumber).IsUnique(true);
        builder.HasIndex(x => x.EmployeeNumber).IsUnique(true);

        builder.HasMany(x => x.Demands)
            .WithOne(x => x.Employee)
            .HasForeignKey(x => x.EmployeeId)
            .IsRequired(true);

        builder.HasMany(x => x.Infos)
            .WithOne(x => x.Employee)
            .HasForeignKey(x => x.EmployeeId)
            .IsRequired(true);
    }
}


