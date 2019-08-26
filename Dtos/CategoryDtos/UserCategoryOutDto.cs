using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog_Project.Dtos.UserDtos;

namespace Blog_Project.Dtos.CategoryDtos
{
    public class UserCategoryOutDto
    {
        public string Id { get; set; }

        public string UserId { get; set; }
        public UserOutDto User { get; set; }
        public string CategoryId { get; set; }
        public CategoryOutDto Category { get; set; }

    }
}
