using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Project.Models
{
    public class BaseModel
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public DateTime SubmitTime { get; set; }

        [Required]
        public DateTime LastUpdateTime { get; set; }

        [Required]
        public long IsDeleted { get; set; }

    }
}
