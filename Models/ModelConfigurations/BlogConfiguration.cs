using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog_Project.Models.ModelConfigurations
{
    public class BlogConfiguration : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder.HasKey(prop => prop.Id);

            builder.Property(prop => prop.Content).HasMaxLength(500);

            builder.Property(prop => prop.LikesCount);

            builder.Property(prop => prop.PostId);


        }
    }
}
