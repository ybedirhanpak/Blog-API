using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Project.Dtos.UserDtos
{
    public class UserUpdateDto
    {
        public string Password { get; set; }

        public string BirthDate { get; set; }

        public string Description { get; set; }

        public string Theme { get; set; }

        public string FacebookLink { get; set; }

        public string TwitterLink { get; set; }

        public string InstagramLink { get; set; }

        public string LinkedinLink { get; set; }
    }
}
