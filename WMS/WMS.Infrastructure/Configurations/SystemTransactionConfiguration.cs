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
    public class SystemTransactionConfiguration : IEntityTypeConfiguration<SystemTransaction>
    {
        public void Configure(EntityTypeBuilder<SystemTransaction> builder)
        {
            builder.ToTable("SystemTransactions").HasKey(st => st.TransactionID);

            // Foreign Keys Properties
            builder.Property(st => st.WarehouseID).IsRequired();
            builder.Property(st => st.ItemID).IsRequired();
            builder.Property(st => st.TransactionTypeID).IsRequired();
            builder.Property(st => st.CreatedBy).IsRequired();

            // Data Properties
            builder.Property(st => st.Quantity).IsRequired();
            builder.Property(st => st.RunningBalance).HasPrecision(18, 2).IsRequired();
            builder.Property(st => st.CreatedAt).HasColumnType("datetime").HasDefaultValueSql("GETDATE()").IsRequired();
            builder.Property(st => st.Description).HasMaxLength(500).IsRequired(false);
            builder.Property(st => st.ReferenceNumber).HasMaxLength(100).IsRequired(false);

            // Relationships 
            builder.HasOne(st => st.WarehouseInfo).WithMany().HasForeignKey(st => st.WarehouseID);

            builder.HasOne(st => st.ItemInfo).WithMany().HasForeignKey(st => st.ItemID);

            builder.HasOne(st => st.TransactionTypeInfo)
                .WithMany()
               .HasForeignKey(st => st.TransactionTypeID)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(st => st.CreatorInfo)
                   .WithMany()
                   .HasForeignKey(st => st.CreatedBy)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
