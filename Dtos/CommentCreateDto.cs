using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Project.Dtos
{
    public class CommentCreateDto
    {
        public Guid OwnerId { get; set; }
        public Guid PostId { get; set; }
        public string Content { get; set; }
    }
}
