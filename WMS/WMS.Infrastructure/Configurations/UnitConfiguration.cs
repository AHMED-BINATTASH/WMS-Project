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
    public class UnitConfiguration : IEntityTypeConfiguration<Unit>
    {
        public void Configure(EntityTypeBuilder<Unit> builder)
        {
            builder.ToTable("Units").HasKey(u => u.UnitID);

            builder.Property(u => u.UnitName)
                   .IsRequired()
                   .HasMaxLength(100); 

            builder.Property(u => u.UnitSymbol)
                   .IsRequired()
                   .HasMaxLength(10);
        }
    }
}
