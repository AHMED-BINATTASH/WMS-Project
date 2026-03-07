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
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories").HasKey(c => c.CategoryID);
            builder.Property(c => c.CategoryName).IsRequired().HasMaxLength(100);

            builder.HasOne(c => c.ParentCategoryInfo)
                   .WithMany()
                   .HasForeignKey(c => c.ParentCategoryId)
                   .OnDelete(DeleteBehavior.NoAction); // this will set Null for parent record and childern will be orphaned
        }
    }
}
