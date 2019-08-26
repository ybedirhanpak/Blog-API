using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Project.Dtos.CategoryDtos
{
    public class MainCategoryOutDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<CategoryOutDto> SubCategories { get; set; }

    }
}
