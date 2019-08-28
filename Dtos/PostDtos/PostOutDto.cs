using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog_Project.Dtos.CategoryDtos;
using Blog_Project.Dtos.CommentDtos;
using Blog_Project.Dtos.UserDtos;
using Blog_Project.Models;

namespace Blog_Project.Dtos.PostDtos
{
    public class PostOutDto : BaseOutDto
    {
        public string OwnerId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public UserOutDto Owner { get; set; }
        public List<CommentOutDto> Comments { get; set; }
        public CategoryOutDto Category { get; set; }
        public List<UserLikePostOutDto> LikedUsers { get; set; }
        public int ViewCount { get; set; }
        public PostOutDto PreviousPost { get; set; }
        public PostOutDto NextPost { get; set; }
    }
}
