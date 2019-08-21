using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Project.Dtos
{
    public class UserCategoryDto
    {
        public Guid UserId { get; set; }
        public Guid CategoryId { get; set; }
    }
}
