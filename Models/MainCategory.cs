using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Project.Models
{
    public class MainCategory
    {
        public MainCategory()
        {
            Id = Guid.NewGuid().ToString();
            IsDeleted = false;
        }

        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        public List<Category> SubCategories { get; set; }

        public bool IsDeleted { get; set; }
    }
}
