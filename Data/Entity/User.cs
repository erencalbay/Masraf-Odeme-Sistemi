using Data.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WebAPI.Entity;
using Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace WebAPI.Entity
{
    [Table("User")]

    public class User : BaseEntity
    {
        public User()
        {
            Roles = new List<Role>();
        }
        public string IdentityNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int UserNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime LastActivityDate { get; set; }

        public virtual List<Info> Infos { get; set; }
        public virtual List<Demand> Demands { get; set; }
        public virtual List<Role> Roles { get; set; }

        public UserRefreshToken UserRefreshToken { get; set; }
    }
}
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(x => x.InsertDate).IsRequired(true);
        builder.Property(x => x.InsertUserNumber).IsRequired(true);
        builder.Property(x => x.UpdateUserNumber).IsRequired(false);
        builder.Property(x => x.UpdateDate).IsRequired(false);
        builder.Property(x => x.isActive).IsRequired(true).HasDefaultValue(true);


        builder.Property(x => x.UserNumber).IsRequired(true).ValueGeneratedNever();
        builder.HasIndex(x => x.UserNumber).IsUnique(true);
        builder.HasKey(x => x.UserNumber);
        builder.Property(x => x.IdentityNumber).IsRequired(true);
        builder.Property(x => x.FirstName).IsRequired(true).HasMaxLength(30);
        builder.Property(x => x.LastName).IsRequired(true).HasMaxLength(30);
        builder.Property(x => x.Email).IsRequired(true).HasDefaultValue(false);
        builder.Property(x => x.UserNumber).IsRequired(true).ValueGeneratedNever();
        builder.Property(x => x.DateOfBirth).IsRequired(true);
        builder.Property(x => x.LastActivityDate).IsRequired(true);

        builder.HasIndex(x => x.IdentityNumber).IsUnique(true);

        builder.HasData(new List<User> { 
            new User { UserNumber = 445566, DateOfBirth = DateTime.UtcNow.AddYears(-20), Email = "erencalbay@gmail.com", FirstName = "Eren", LastName = "Çalbay", Roles = new List<Role>(), IdentityNumber = "44332211002"},
            new User { UserNumber = 112233, DateOfBirth = DateTime.UtcNow.AddYears(-15), Email = "ahmetkızılkaya@gmail.com", FirstName = "Ahmet", LastName = "Kızılkaya", Roles = new List<Role>(), IdentityNumber = "34332211002"}});

        builder.HasMany(x => x.Demands)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserNumber)
            .IsRequired(true);

        builder.HasMany(x => x.Infos)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserNumber)
            .IsRequired(true);

    }
}


