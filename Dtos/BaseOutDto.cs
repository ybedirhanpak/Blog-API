using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Project.Dtos
{
    public class BaseOutDto
    {
        public string Id { get; set; }
        public DateTime SubmitTime { get; set; }
        public DateTime LastUpdateTime { get; set; }

    }
}
