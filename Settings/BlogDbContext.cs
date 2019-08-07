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
        DbSet<User> Comments { get; set; }
        DbSet<User> Categories { get; set; }
    }
}
