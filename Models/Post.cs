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
            SubmitDate = DateTime.Now.ToString();
        }

        
        public Guid Id { get; set; }

        public Guid OwnerId { get; set; }
        public User Owner { get; set; }

        public string Title { get; set; }

        public List<Comment> Comments { get; set; }

        public string Content { get; set; }

        public List<PostCategory> RelatedCategories { get; set; }

        public string SubmitDate { get; set; }

        public string LastUpdateDate { get; set; }

        public List<UserLikePost> LikedUsers { get; set; }

        public Guid? PreviousPostId { get; set; }
        public Post PreviousPost { get; set; }

        public Guid? NextPostId { get; set; }
        public Post NextPost { get; set; }

        public int ViewCount { get; set; }

    }
}
