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
    public class ItemUnitConfiguration : IEntityTypeConfiguration<ItemUnit>
    {
        public void Configure(EntityTypeBuilder<ItemUnit> builder)
        {
            builder.ToTable("ItemUnits").HasKey(iu => iu.ItemUnitID);

            builder.Property(iu => iu.ItemUnitID)
                   .ValueGeneratedOnAdd();

            builder.Property(iu => iu.Factor)
                   .IsRequired()
                   .HasPrecision(18, 4);

            builder.Property(iu => iu.ItemID)
                .IsRequired();

            builder.Property(iu => iu.UnitID)
                .IsRequired();

            builder.HasOne(iu => iu.ItemInfo)
                   .WithMany() 
                   .HasForeignKey(iu => iu.ItemID)
                   .OnDelete(DeleteBehavior.Cascade); 
        
            builder.HasOne(iu => iu.UnitInfo)
                   .WithMany()
                   .HasForeignKey(iu => iu.UnitID)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
