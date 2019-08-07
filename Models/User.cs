using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Project.Models
{
    public class User
    {

        public User()
        {
            Id = new Guid();
        }

        public Guid Id { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public Guid[] Posts { get; set; }

        public Guid[] Followings { get; set; }

        public Guid[] Followers { get; set; }

        public string BirthDate { get; set; }

        public string RegistrationDate { get; set; }

        public string Description { get; set; }

        public Guid[] InterestedCategories { get; set; }

        public string Theme { get; set; }

        public string FacebookLink { get; set; }

        public string TwitterLink { get; set; }

        public string InstagramLink { get; set; }

        public string LinkedinLink { get; set; }
    }
}
