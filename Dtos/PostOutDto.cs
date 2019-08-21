using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog_Project.Models;

namespace Blog_Project.Dtos
{
    public class PostOutDto
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public User Owner { get; set; }
        public int ViewCount { get; set; }
        public DateTime SubmitDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public Post PreviousPost { get; set; }
        public Post NextPost { get; set; }
    }
}
