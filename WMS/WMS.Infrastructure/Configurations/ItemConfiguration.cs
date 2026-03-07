using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Domain.Entities;

namespace WMS.Infrastructure.Configurations
{
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.ToTable("Items").HasKey(i => i.ItemID);

            builder.Property(i => i.ItemName)
                   .IsRequired()
                   .HasMaxLength(250);

            builder.Property(i => i.Barcode)
                   .HasMaxLength(100);

            builder.Property(i => i.AverageCost)
                   .HasPrecision(18, 2);

            builder.Property(i => i.ReorderPoint)
                   .HasPrecision(18, 2);

            builder.Property(i => i.IsActive)
                   .HasDefaultValue(true);

            builder.Property(i => i.IsExpiryRelated)
                   .HasDefaultValue(false);

            builder.Property(i => i.CreatedAt)
                   .HasColumnType("DATETIME")
                   .HasDefaultValueSql("GETDATE()");

            builder.HasOne(i => i.CategoryInfo)
                   .WithMany() 
                   .HasForeignKey(i => i.CategoryID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(i => i.UnitInfo)
                   .WithMany()
                   .HasForeignKey(i => i.UnitID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(i => i.CeatorInfo)
                   .WithMany()
                   .HasForeignKey(i => i.CreatedBy)
                   .OnDelete(DeleteBehavior.NoAction);

            
            // This automatically excludes "deleted" items from all queries
            builder.HasQueryFilter(i => i.IsActive);  
        }
    }
}
