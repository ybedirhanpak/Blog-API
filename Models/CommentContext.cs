using Blog_Project.Models.ModelConfigurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;


namespace Blog_Project.Models
{
    public class CommentContext: DbContext
    {
        public CommentContext(DbContextOptions<CommentContext> options)
            :base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.ApplyConfiguration(new CommentConfiguration());

        public DbSet<Comment> Comment { get; set; }

    }
}
