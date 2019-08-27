using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Project.Models
{
    public class Post : BaseModel
    {

        public Post()
        {
            Id = Guid.NewGuid().ToString();
            ViewCount = 0;
        }

        [Required]
        public string OwnerId { get; set; }
        public User Owner { get; set; }

        [Required]
        public string Title { get; set; }

        public List<Comment> Comments { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string CategoryId { get; set; }
        public Category Category { get; set; }

        public List<UserLikePost> LikedUsers { get; set; }

        public int ViewCount { get; set; }

        public string[] Tags { get; set; }
    }
}
