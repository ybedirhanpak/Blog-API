using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Blog_Project.Models
{
    public class Comment
    {
        public Comment()
        {
            SubmitDate = DateTime.Now;
            LastEditTime = SubmitDate;
            Id = Guid.NewGuid().ToString();
            LikeCount = 0;
            IsDeleted = false;
        }

        [Key]
        public string Id { get; set; }

        [Required]
        public string OwnerId { get; set; }
        public User Owner { get; set; }

        [Required]
        public string PostId { get; set; }
        public Post Post { get; set; }

        [Required]
        public DateTime SubmitDate { get; set; }

        public DateTime LastEditTime { get; set; }

        [Required]
        public string Content { get; set; }

        public int LikeCount { get; set; }

        public bool IsDeleted { get; set; }
    }

}
