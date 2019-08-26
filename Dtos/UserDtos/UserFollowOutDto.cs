using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Project.Dtos.UserDtos
{
    public class UserFollowOutDto
    {
        public string FollowerId { get; set; }
        public UserOutDto Follower { get; set; }
        public string FollowedId { get; set; }
        public UserOutDto Followed { get; set; }
    }
}
