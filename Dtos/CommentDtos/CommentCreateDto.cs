using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Project.Dtos.CommentDtos
{
    public class CommentCreateDto
    {
        public string OwnerId { get; set; }
        public string PostId { get; set; }
        public string Content { get; set; }
    }
}
