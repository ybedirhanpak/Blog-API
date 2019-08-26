using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Project.Models
{
    public class UserLikePost
    {
        public UserLikePost()
        {
            Id = Guid.NewGuid().ToString();
        }

        public UserLikePost(string userId, string postId)
        {
            Id = Guid.NewGuid().ToString();
            UserId = userId;
            PostId = postId;
        }

        [Key]
        public string Id { get; set; }

        [Required]
        public string UserId { get; set; }
        public User User { get; set; }

        [Required]
        public string PostId { get; set; }
        public Post Post { get; set; }

    }
}
