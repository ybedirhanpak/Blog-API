using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Project.Dtos.CategoryDtos
{
    public class CategoryInDto
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string ParentId { get; set; }
    }
}
