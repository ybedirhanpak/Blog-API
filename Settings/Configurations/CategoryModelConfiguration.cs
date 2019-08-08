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
            builder.HasOne(c => c.Parent)
                .WithMany(c => c.Children)
                .HasForeignKey(c => c.ParentId);
        }

    }
}
