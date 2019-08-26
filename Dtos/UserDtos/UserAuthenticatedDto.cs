using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Blog_Project.Dtos.CategoryDtos;
using Blog_Project.Dtos.PostDtos;
using Blog_Project.Models;

namespace Blog_Project.Dtos.UserDtos
{
    public class UserAuthenticatedDto
    {
        public string Id { get; set; }
        public string Role { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public List<PostOutDto> Posts { get; set; }

        public List<UserLikePostOutDto> LikedPosts { get; set; }

        public List<UserFollowOutDto> Followings { get; set; }

        public List<UserFollowOutDto> Followers { get; set; }

        public string BirthDate { get; set; }

        public DateTime RegistrationDate { get; set; }

        public string Description { get; set; }

        public List<UserCategoryOutDto> InterestedCategories { get; set; }

        public string Theme { get; set; }

        public string FacebookLink { get; set; }

        public string TwitterLink { get; set; }

        public string InstagramLink { get; set; }

        public string LinkedinLink { get; set; }

        public string Token { get; set; }

    }
}
