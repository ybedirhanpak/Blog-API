using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog_Project.Dtos.CategoryDtos;

namespace Blog_Project.Dtos.PostDtos
{
    public class PostCategoryOutDto
    {
        public string Id { get; set; }

        public string PostId { get; set; }
        public PostOutDto Post { get; set; }

        public string CategoryId { get; set; }
        public CategoryOutDto Category { get; set; }

    }
}
