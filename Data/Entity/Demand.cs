﻿using Base.Entity;
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
    // Talep entitiyleri
    [Table("Demand")]
    public class Demand : BaseEntity
    {
        public int DemandId { get; set; }
        public int UserNumber { get; set; }
        public virtual User User { get; set; }
        public string Description { get; set; }
        public int DemandNumber { get; set; }
        public string Receipt { get; set; }
        public DemandResponseType DemandResponseType { get; set; }
        public string RejectionResponse { get; set; }
    }
}

// Talepler için DB validasyon ve bağlantı işlemleri 
public class DemandConfiguration : IEntityTypeConfiguration<Demand>
{
    public void Configure(EntityTypeBuilder<Demand> builder)
    {
        builder.Property(x => x.InsertDate).IsRequired(true);
        builder.Property(x => x.InsertUserNumber).IsRequired(true);
        builder.Property(x => x.UpdateUserNumber).IsRequired(false);
        builder.Property(x => x.UpdateDate).IsRequired(false);
        builder.Property(x => x.Receipt).IsRequired(true);
        
        builder.Property(x => x.DemandId).IsRequired(true);
        builder.HasKey(x => x.DemandId);
        builder.HasIndex(x => x.DemandId).IsUnique(true);
        builder.Property(x => x.DemandNumber).IsRequired(true);
        builder.Property(x => x.UserNumber).IsRequired(true);
        builder.Property(x => x.Description).IsRequired(true).HasMaxLength(1000);
        builder.Property(x => x.isActive).IsRequired(true).HasDefaultValue(true);
        builder.Property(x => x.DemandResponseType).IsRequired(true);
        builder.Property(x => x.RejectionResponse).IsRequired(false);

        builder.HasData(new List<Demand> {
        });
    
        builder.HasIndex(x => x.UserNumber);
    }
}