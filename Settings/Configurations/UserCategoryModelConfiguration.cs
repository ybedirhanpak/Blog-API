using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog_Project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog_Project.Settings.Configurations
{
    public class UserCategoryModelConfiguration : IEntityTypeConfiguration<UserCategory>
    {
        public void Configure(EntityTypeBuilder<UserCategory> builder)
        {

            builder.HasOne(uc => uc.User)
                .WithMany(u => u.InterestedCategories)
                .HasForeignKey(uc => uc.UserId);


            builder.HasOne(uc => uc.Category)
                .WithMany(c => c.FollowerUsers)
                .HasForeignKey(uc => uc.CategoryId);

        }

    }
}
