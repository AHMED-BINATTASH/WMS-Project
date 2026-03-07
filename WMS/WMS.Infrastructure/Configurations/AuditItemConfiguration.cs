using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Domain.Entities;

namespace WMS.Infrastructure.Configurations
{
    public class AuditItemConfiguration : IEntityTypeConfiguration<AuditItem>
    {
        public void Configure(EntityTypeBuilder<AuditItem> builder)
        {
            builder.ToTable("AuditItem").HasKey(a => a.AuditItemID);

            builder.Property(a => a.WarehouseId)
                .IsRequired();

            builder.Property(a => a.ItemId)
                .IsRequired();

            builder.Property(a => a.FieldName).HasMaxLength(255).IsRequired();

            builder.Property(a => a.OldValue).HasMaxLength(255).IsRequired();

            builder.Property(a => a.CreatedBy);

            builder.Property(a => a.CreatedAt).HasColumnType("DATETIME")
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

            builder.Property(a => a.ActionType).HasMaxLength(255).IsRequired();

            builder.HasOne(a => a.Warehouse)
                .WithMany()
                .HasForeignKey(a => a.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Item)
                .WithMany()
                .HasForeignKey(a => a.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.CreatorInfo)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

