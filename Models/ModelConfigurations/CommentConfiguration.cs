using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog_Project.Models.ModelConfigurations
{
    public class CommentConfiguration: IEntityTypeConfiguration<Comment>
    {


        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(prop => prop.Id);

            builder.Property(prop => prop.Content).HasMaxLength(500);

            builder.Property(prop => prop.LikeCount);

            builder.Property(prop => prop.PostId);

            builder.Property(prop => prop.Date);



        }
    }
}
