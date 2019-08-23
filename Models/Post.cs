using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Project.Models
{
    public class Post
    {

        public Post()
        {
            Id = Guid.NewGuid();
            SubmitDate = DateTime.Now;
            LastUpdateDate = SubmitDate;
            ViewCount = 0;
            IsDeleted = false;
        }

        public Guid Id { get; set; }

        [Required]
        public Guid OwnerId { get; set; }
        public User Owner { get; set; }

        [Required]
        public string Title { get; set; }

        public List<Comment> Comments { get; set; }

        [Required]
        public string Content { get; set; }

        public List<PostCategory> RelatedCategories { get; set; }

        [Required]
        public DateTime SubmitDate { get; set; }

        public DateTime LastUpdateDate { get; set; }

        public List<UserLikePost> LikedUsers { get; set; }

        public int ViewCount { get; set; }

        public bool IsDeleted { get; set; }
        
        public string[] Tags { get; set; }
    }
}
