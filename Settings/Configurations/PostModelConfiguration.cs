using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog_Project.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog_Project.Settings.Configurations
{
    public class PostModelConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasQueryFilter(x => x.IsDeleted == 0L);

            builder.HasOne(p => p.Owner)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.OwnerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Category)
                .WithMany(c => c.RelatedPosts)
                .HasForeignKey(p => p.CategoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
