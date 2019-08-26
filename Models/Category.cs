using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Project.Models
{
    public class Category
    {
        public Category()
        {
            Id = Guid.NewGuid().ToString();
            IsDeleted = false;
        }

        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public List<PostCategory> RelatedPosts { get; set; }

        public string ParentId { get; set; }
        public MainCategory Parent { get; set; }

        public List<UserCategory> FollowerUsers { get; set; }

        public bool IsDeleted { get; set; }
    }
}
