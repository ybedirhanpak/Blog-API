using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Blog_Project.Models
{
    public class BlogDbContext : DbContext
    {

        public BlogDbContext(DbContextOptions options) : base(options)
        {
        }
        DbSet<Post> Posts { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<Comment> Comments { get; set; }
        DbSet<Category> Categories { get; set; }
    }
}
