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
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable("People").HasKey(p => p.PersonID);

            builder.Property(p => p.NationalID).HasMaxLength(50).IsRequired();
            builder.HasIndex(p => p.NationalID).IsUnique();

            builder.Property(p => p.FirstName).HasMaxLength(50).IsRequired();

            builder.Property(p => p.LastName).HasMaxLength(50).IsRequired();

            builder.Property(p => p.Address).HasMaxLength(200).IsRequired();

            builder.Property(p => p.Phone).HasMaxLength(20).IsRequired();

            builder.Property(p => p.Email).HasMaxLength(50).IsRequired();
            builder.HasIndex(p => p.Email).IsUnique();

            builder.HasOne(p => p.CountryInfo)
                .WithMany()
                .HasForeignKey(p => p.CountryID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
