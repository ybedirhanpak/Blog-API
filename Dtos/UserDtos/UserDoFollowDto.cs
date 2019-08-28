using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Project.Dtos.UserDtos
{
    public class UserDoFollowDto
    {
        public string FollowerId { get; set; }

        public string FollowedId { get; set; }
    }
}
