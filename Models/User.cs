using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Project.Models
{
    public class User : BaseModel
    {
        public User()
        {
            Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string Role { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }

        [Required]
        public byte[] PasswordSalt { get; set; }

        public List<Post> Posts { get; set; }

        public List<UserLikePost> LikedPosts { get; set; }

        public List<UserFollow> Followings { get; set; }
        
        public List<UserFollow> Followers { get; set; }

        public string BirthDate { get; set; }

        public string Description { get; set; }

        public List<UserCategory> InterestedCategories { get; set; }

        public string Theme { get; set; }

        public string FacebookLink { get; set; }

        public string TwitterLink { get; set; }

        public string InstagramLink { get; set; }

        public string LinkedinLink { get; set; }

    }
}
