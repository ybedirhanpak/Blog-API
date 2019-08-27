using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Project.Models
{
    public class MainCategory : BaseModel
    {
        public MainCategory()
        {
            Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string Name { get; set; }

        public List<Category> SubCategories { get; set; }

    }
}
