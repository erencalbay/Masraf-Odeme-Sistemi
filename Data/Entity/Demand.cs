using Base.Entity;
using Data.Entity;
using Data.Enum;
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
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
        public string Description { get; set; }
        public bool isActive { get; set; }
        public DemandType DemandType { get; set; }
        public string RejectionResponse { get; set; }
    }
}

public class DemandConfiguration : IEntityTypeConfiguration<Demand>
{
    public void Configure(EntityTypeBuilder<Demand> builder)
    {
        builder.Property(x => x.InsertDate).IsRequired(true);
        builder.Property(x => x.InsertUserId).IsRequired(true);
        builder.Property(x => x.UpdateUserId).IsRequired(false);
        builder.Property(x => x.UpdateDate).IsRequired(false);

        builder.Property(x => x.EmployeeId).IsRequired(true);
        builder.Property(x => x.Description).IsRequired(true).HasMaxLength(1000);
        builder.Property(x => x.isActive).IsRequired(true);
        builder.Property(x => x.DemandType).IsRequired(true);
        builder.Property(x => x.RejectionResponse).IsRequired(false);
    
        builder.HasIndex(x => x.EmployeeId);
    }
}