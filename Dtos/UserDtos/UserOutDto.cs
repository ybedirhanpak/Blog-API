using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog_Project.Models;

namespace Blog_Project.Dtos.UserDtos
{
    public class UserOutDto
    {
        public Guid Id { get; set; }
        public string Role { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public List<Post> Posts { get; set; }

        public List<UserLikePost> LikedPosts { get; set; }

        public List<UserFollow> Followings { get; set; }

        public List<UserFollow> Followers { get; set; }

        public string BirthDate { get; set; }

        public DateTime RegistrationDate { get; set; }

        public string Description { get; set; }

        public List<UserCategory> InterestedCategories { get; set; }

        public string Theme { get; set; }

        public string FacebookLink { get; set; }

        public string TwitterLink { get; set; }

        public string InstagramLink { get; set; }

        public string LinkedinLink { get; set; }

    }
}
