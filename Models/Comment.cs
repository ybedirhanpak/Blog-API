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
            Id = new Guid();
            LikeCount = 0;
            IsDeleted = false;
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid OwnerId { get; set; }
        public User Owner { get; set; }

        [Required]
        public Guid PostId { get; set; }
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
