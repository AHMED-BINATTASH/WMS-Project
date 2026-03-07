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
    public class WarehouseStockConfiguration : IEntityTypeConfiguration<WarehouseStock>
    {
        public void Configure(EntityTypeBuilder<WarehouseStock> builder)
        {
            builder.ToTable("WarehouseStocks").HasKey(ws => ws.WarehouseStockID);

            builder.Property(ws => ws.Quantity)
                   .IsRequired(); 

            builder.Property(ws => ws.BatchNumber)
                   .HasMaxLength(100); 

            builder.Property(ws => ws.ActualCost)
                   .IsRequired()
                   .HasPrecision(18, 4); 

            builder.Property(ws => ws.ProductionDate)
                   .HasColumnType("DATE");

            builder.Property(ws => ws.ExpiryDate)
                   .HasColumnType("DATE");

            builder.Property(ws => ws.CreatedAt)
                   .HasColumnType("DATETIME")
                   .HasDefaultValueSql("GETDATE()");

            builder.HasOne(ws => ws.WarehouseInfo)
                   .WithMany()
                   .HasForeignKey(ws => ws.WarehouseID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ws => ws.ItemInfo)
                   .WithMany()
                   .HasForeignKey(ws => ws.ItemID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ws => ws.CreatorInfo)
                   .WithMany()
                   .HasForeignKey(ws => ws.CreatedBy) 
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
