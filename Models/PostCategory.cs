using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Project.Models
{
    public class PostCategory
    {

        public PostCategory(Guid postId, Guid categoryId)
        {
            PostId = postId;
            CategoryId = categoryId;
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid PostId { get; set; }
        public Post Post { get; set; }

        [Required]
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

    }
}
