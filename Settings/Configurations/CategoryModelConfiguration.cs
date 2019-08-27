using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog_Project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog_Project.Settings.Configurations
{
    public class CategoryModelConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasQueryFilter(x => x.IsDeleted == 0L);

            builder.HasIndex(c => c.Name).IsUnique();

            builder.HasOne(c => c.Parent)
                .WithMany(mc => mc.SubCategories)
                .HasForeignKey(c => c.ParentId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
