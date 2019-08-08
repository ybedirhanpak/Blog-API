using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog_Project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog_Project.Settings.Configurations
{
    public class PostCategoryModelConfiguration : IEntityTypeConfiguration<PostCategory>
    {
        public void Configure(EntityTypeBuilder<PostCategory> builder)
        {
            builder.HasOne(pc => pc.Post)
                .WithMany(p => p.RelatedCategories)
                .HasForeignKey(pc => pc.PostId);

            builder.HasOne(pc => pc.Category)
                .WithMany(c => c.RelatedPosts)
                .HasForeignKey(pc => pc.CategoryId);
        }

    }
}
