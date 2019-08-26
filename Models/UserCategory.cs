using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Project.Models
{
    public class UserCategory
    {

        public UserCategory()
        {
            Id = Guid.NewGuid().ToString();
        }

        public UserCategory(string userId, string categoryId)
        {
            Id = Guid.NewGuid().ToString();
            UserId = userId;
            CategoryId = categoryId;
        }

        [Key]
        public string Id { get; set; }

        [Required]
        public string UserId { get; set; }
        public User User { get; set; }

        [Required]
        public string CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
