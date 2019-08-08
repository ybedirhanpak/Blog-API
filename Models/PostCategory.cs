using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Project.Models
{
    public class PostCategory
    {
        [Key]
        public Guid Id { get; set; }

        public Guid PostId { get; set; }
        public Post Post { get; set; }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

    }
}
