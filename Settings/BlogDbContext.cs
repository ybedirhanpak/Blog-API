using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blog_Project.Settings.Configurations;
using Microsoft.EntityFrameworkCore;
using Blog_Project.Models;

namespace Blog_Project.Settings
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
            modelBuilder.ApplyConfiguration(new CommentModelConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryModelConfiguration());
            modelBuilder.ApplyConfiguration(new MainCategoryModelConfiguration());
            modelBuilder.ApplyConfiguration(new UserFollowModelConfiguration());
            modelBuilder.ApplyConfiguration(new UserCategoryModelConfiguration());
            modelBuilder.ApplyConfiguration(new UserLikePostModelConfiguration());
        }

        DbSet<Post> Posts { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<UserFollow> UserFollows { get; set; }
        DbSet<Comment> Comments { get; set; }
        DbSet<MainCategory> MainCategories { get; set; }
        DbSet<Category> Categories { get; set; }

        public override int SaveChanges()
        {
            OnBeforeSaving();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues["IsDeleted"] = 0L;
                        entry.CurrentValues["SubmitTime"] = DateTime.UtcNow;
                        entry.CurrentValues["LastUpdateTime"] = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.CurrentValues["LastUpdateTime"] = DateTime.UtcNow;
                        break;
                    case EntityState.Deleted:
                        entry.CurrentValues["IsDeleted"] = DateTime.UtcNow.Ticks;
                        break;
                }
            }
        }
    }
}
