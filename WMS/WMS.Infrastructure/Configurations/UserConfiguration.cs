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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users").HasKey(u => u.UserID);

            builder.Property(u => u.Username).HasMaxLength(128).IsRequired();
            builder.HasIndex(u => u.Username).IsUnique();

            builder.Property(u => u.Role).HasMaxLength(128).IsRequired();

            builder.Property(u => u.Password).HasColumnType("VARCHAR").HasMaxLength(255).IsRequired();

            builder.Property(u => u.IsActive).HasColumnType("BIT").HasDefaultValue(true).IsRequired();

            builder.HasOne(u => u.PersonInfo)
                .WithOne()
                .HasForeignKey<User>(u => u.PersonID);
        }
    }
}
