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
    public class TransactionTypeConfiguration : IEntityTypeConfiguration<TransactionType>
    {
        public void Configure(EntityTypeBuilder<TransactionType> builder)
        {
            builder.ToTable("TransactionTypes").HasKey(tt => tt.TransactionTypeID);

            builder.Property(tt => tt.TransactionTypeName)
               .IsRequired() 
               .HasMaxLength(100); 

            builder.Property(tt => tt.Description)
               .HasMaxLength(500); 
        }
    }
}
