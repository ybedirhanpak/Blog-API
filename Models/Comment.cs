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
            Date = DateTime.Now;
            Id = new Guid();
        }

        [Key]
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public User Owner { get; set; }
        public Guid PostId { get; set; }
        public Post Post { get; set; }

        public DateTime Date { get; set; }
        public string Content { get; set; }
        public int LikeCount { get; set; }

    }

}
