using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog_Project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog_Project.Settings.Configurations
{
    public class UserLikePostModelConfiguration : IEntityTypeConfiguration<UserLikePost>
    {
        public void Configure(EntityTypeBuilder<UserLikePost> builder)
        {
            builder.HasOne(up => up.User)
                .WithMany(u => u.LikedPosts)
                .HasForeignKey(up => up.UserId);

            builder.HasOne(up => up.Post)
                .WithMany(p => p.LikedUsers)
                .HasForeignKey(up => up.PostId);
        }

    }
}
