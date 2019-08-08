using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog_Project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog_Project.Settings.Configurations
{
    public class CommentModelConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {

            builder.HasOne(c => c.Owner)
                .WithMany()
                .HasForeignKey(c => c.OwnerId).IsRequired();

            builder.HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId).IsRequired();

        }

    }
}
