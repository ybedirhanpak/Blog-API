using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog_Project.Dtos.PostDtos;

namespace Blog_Project.Dtos.CategoryDtos
{
    public class CategoryOutDto : BaseOutDto
    {
        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public List<PostOutDto> RelatedPosts { get; set; }

        public string ParentId { get; set; }
        public MainCategoryOutDto Parent { get; set; }

        public List<UserCategoryOutDto> FollowerUsers { get; set; }

    }
}
