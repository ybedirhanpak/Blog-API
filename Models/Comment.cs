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
        public string OwnerId { get; set; }
        public string PostId { get; set; }
        public DateTime Date { get; set; }
        public string Content { get; set; }
        public int LikeCount { get; set; }

    }

}
