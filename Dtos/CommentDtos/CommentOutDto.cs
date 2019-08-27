using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog_Project.Dtos.PostDtos;
using Blog_Project.Dtos.UserDtos;

namespace Blog_Project.Dtos.CommentDtos
{
    public class CommentOutDto : BaseOutDto
    {
        public string OwnerId { get; set; }
        public UserOutDto Owner { get; set; }
        public string PostId { get; set; }
        public PostOutDto Post { get; set; }
        public string Content { get; set; }
        public int LikeCount { get; set; }
    }
}
