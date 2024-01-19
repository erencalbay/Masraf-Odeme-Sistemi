using Base.Entity;
using Data.Entity;
using Data.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Entity;

namespace Data.Entity
{
    [Table("Demand", Schema = "dbo")]
    public class Demand : BaseEntity
    {
        public int DemandId { get; set; }
        public int UserNumber { get; set; }
        public virtual User User { get; set; }
        public string Description { get; set; }
        public bool isDefault { get; set; }
        public int DemandNumber { get; set; }
        public DemandType DemandType { get; set; }
        public string RejectionResponse { get; set; }
    }
}

public class DemandConfiguration : IEntityTypeConfiguration<Demand>
{
    public void Configure(EntityTypeBuilder<Demand> builder)
    {
        builder.Property(x => x.InsertDate).IsRequired(true);
        builder.Property(x => x.InsertUserNumber).IsRequired(true);
        builder.Property(x => x.UpdateUserNumber).IsRequired(false);
        builder.Property(x => x.UpdateDate).IsRequired(false);
        
        builder.Property(x => x.DemandId).IsRequired(true);
        builder.HasKey(x => x.DemandId);
        builder.HasIndex(x => x.DemandId).IsUnique(true);
        builder.Property(x => x.DemandNumber).IsRequired(true);
        builder.Property(x => x.UserNumber).IsRequired(true);
        builder.Property(x => x.Description).IsRequired(true).HasMaxLength(1000);
        builder.Property(x => x.isDefault).IsRequired(true);
        builder.Property(x => x.isActive).IsRequired(true).HasDefaultValue(true);
        builder.Property(x => x.DemandType).IsRequired(true);
        builder.Property(x => x.RejectionResponse).IsRequired(false);
    
        builder.HasIndex(x => x.UserNumber);
    }
}