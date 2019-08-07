using System;
using System.Collections.Generic;
using System.Text;

namespace Blog_Project.Models
{
    public class Blog
    {
        public Blog()
        {
        }
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string Content { get; set; }
        public int LikesCount { get; set; }
        public int PostId { get; set; }
    }

    
}
