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
            builder.HasOne(p => p.Owner)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.OwnerId);

            builder.HasOne(p => p.PreviousPost)
                .WithOne(p => p.NextPost)
                .HasForeignKey(typeof(Post), "NextPostId");
                

        }

    }
}
