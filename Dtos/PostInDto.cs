using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Project.Dtos
{
    public class PostInDto
    {
        public Guid OwnerId { get; set; }
        public Guid CategoryId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
