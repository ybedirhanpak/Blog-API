using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Project.Models
{
    public class Post
    {
        public Guid Id { get; set; }

        public Guid OwnerId { get; set; }

        public string Title { get; set; }

        public Guid[] Comments { get; set; }

        public string Content { get; set; }

        public Guid[] RelatedCategories { get; set; }

        public string SubmitDate { get; set; }

        public string LastUpdateDate { get; set; }

        public Guid[] LikedUsers { get; set; }

        public Guid PreviousPost { get; set; }

        public Guid NextPost { get; set; }

        public int ViewCount { get; set; }

        public Guid LikedPosts { get; set; }

    }
}
