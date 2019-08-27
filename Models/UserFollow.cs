using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Project.Models
{
    public class UserFollow : BaseModel
    {
        public UserFollow()
        {
            Id = Guid.NewGuid().ToString();
        }

        public UserFollow (string followerId, string followedId)
        {
            Id = Guid.NewGuid().ToString();
            FollowerId = followerId;
            FollowedId = followedId;
        }

        [Required]
        public string FollowerId { get; set; }
        public User Follower { get; set; }

        [Required]
        public string FollowedId { get; set; }
        public User Followed { get; set; }
    }
        
}
