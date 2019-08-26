using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Project.Dtos.UserDtos
{
    public class UserLikePostInDto
    {
        public string UserId { get; set; }
        public string PostId { get; set; }
    }
}
