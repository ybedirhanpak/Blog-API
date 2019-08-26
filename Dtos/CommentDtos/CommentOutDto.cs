using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog_Project.Dtos.PostDtos;
using Blog_Project.Dtos.UserDtos;

namespace Blog_Project.Dtos.CommentDtos
{
    public class CommentOutDto
    {
        public string Id { get; set; }

        public string OwnerId { get; set; }
        public UserOutDto Owner { get; set; }

        public string PostId { get; set; }
        public PostOutDto Post { get; set; }

        public DateTime SubmitDate { get; set; }

        public DateTime LastEditTime { get; set; }

        public string Content { get; set; }

        public int LikeCount { get; set; }

    }
}
