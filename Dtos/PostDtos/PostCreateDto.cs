using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Project.Dtos.PostDtos
{
    public class PostCreateDto
    {
        public string OwnerId { get; set; }
        public string CategoryId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
