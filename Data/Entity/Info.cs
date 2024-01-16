using Data.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Entity;
using Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entity
{
    [Table("Info", Schema = "dbo")]
    public class Info : BaseEntity
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public string IBAN { get; set; }
        public string Information { get; set; }
        public int InfoNumber { get; set; }
        public string InfoType { get; set; }
        public bool isDefault { get; set; }
    }
}

public class InfoConfiguration : IEntityTypeConfiguration<Info>
{
    public void Configure(EntityTypeBuilder<Info> builder)
    {
        builder.Property(x => x.InsertDate).IsRequired(true);
        builder.Property(x => x.InsertUserId).IsRequired(true);
        builder.Property(x => x.UpdateUserId).IsRequired(false);
        builder.Property(x => x.UpdateDate).IsRequired(false);


        builder.Property(x => x.InfoNumber).IsRequired(true);
        builder.HasKey(x => x.InfoNumber);
        builder.HasIndex(x => x.InfoNumber).IsUnique(true);
        builder.Property(x => x.UserId).IsRequired(true);
        builder.Property(x => x.IBAN).IsRequired(true).HasMaxLength(30);
        builder.Property(x => x.InfoType).IsRequired(true);
        builder.Property(x => x.isDefault).IsRequired(true).HasDefaultValue(false);
        builder.Property(x => x.Information).IsRequired(true);

        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => new { x.Information, x.InfoType }).IsUnique(true);
    }
}
