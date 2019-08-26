using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog_Project.Dtos.PostDtos;

namespace Blog_Project.Dtos.UserDtos
{
    public class UserLikePostOutDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public UserOutDto User { get; set; }
        public string PostId { get; set; }
        public PostOutDto Post { get; set; }
    }
}
