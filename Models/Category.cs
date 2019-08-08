using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Project.Models
{
    public class Category
    {
        public Category()
        {
            Id = new Guid();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid ParentId { get; set; }

        public Guid[] ChildrenId { get; set; }

        public UserCategory[] FollowerUsers { get; set; }

    }
}
