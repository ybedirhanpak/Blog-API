using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Blog_Project.Models
{
    public class Comment : BaseModel
    {
        public Comment()
        {
            Id = Guid.NewGuid().ToString();
            LikeCount = 0;
        }

        [Required]
        public string OwnerId { get; set; }
        public User Owner { get; set; }

        [Required]
        public string PostId { get; set; }
        public Post Post { get; set; }

        [Required]
        public string Content { get; set; }

        public int LikeCount { get; set; }
    }

}
