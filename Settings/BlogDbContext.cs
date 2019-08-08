using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog_Project.Settings.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Blog_Project.Models
{
    public class BlogDbContext : DbContext
    {

        public BlogDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserModelConfiguration());
            modelBuilder.ApplyConfiguration(new PostModelConfiguration());
            modelBuilder.ApplyConfiguration(new UserFollowModelConfiguration());
            modelBuilder.ApplyConfiguration(new UserCategoryModelConfiguration());
        }

        DbSet<Post> Posts { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<UserFollow> UserFollows { get; set; }
        DbSet<Comment> Comments { get; set; }
        DbSet<Category> Categories { get; set; }

        
    }
}
