using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Domain.Entities;

namespace WMS.Infrastructure.Configurations
{
    public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
    {
        public void Configure(EntityTypeBuilder<Warehouse> builder)
        {
            builder.ToTable("Warehouses").HasKey(w => w.WarehouseID);

            builder.Property(w => w.WarehouseCode)
                   .IsRequired()
                   .HasMaxLength(50); 

            builder.Property(w => w.WarehouseName)
                   .IsRequired()
                   .HasMaxLength(250); 

            builder.Property(w => w.Location)
                   .HasMaxLength(500); 

            builder.Property(w => w.IsActive)
                   .IsRequired()
                   .HasDefaultValue(true); 

            builder.HasQueryFilter(w => w.IsActive);
        }
    }
}
