using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Project.Models
{
    public class PostCategory
    {
        public PostCategory()
        {
            Id = Guid.NewGuid().ToString();

        }

        public PostCategory(string postId, string categoryId)
        {
            Id = Guid.NewGuid().ToString();
            PostId = postId;
            CategoryId = categoryId;

        }

        [Key]
        public string Id { get; set; }

        [Required]
        public string PostId { get; set; }
        public Post Post { get; set; }

        [Required]
        public string CategoryId { get; set; }
        public Category Category { get; set; }

    }
}
