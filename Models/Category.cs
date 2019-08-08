using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Project.Models
{
    public class Category
    {
        public Category()
        {
            Id = new Guid();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<PostCategory> RelatedPosts { get; set; }

        public Guid? ParentId { get; set; }
        public Category Parent { get; set; }

        public List<Category> Children { get; set; }

        public List<UserCategory> FollowerUsers { get; set; }

    }
}
