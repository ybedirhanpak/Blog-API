using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogProjectData.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blog_Project.Controllers
{
    [Route("api/[controller]")]
    public class BlogController : Controller
    {
        readonly BlogContext Context;

        public BlogController(BlogContext context)
        {
            Context = context;
        }

        [HttpGet]
        public IActionResult GetComment()
        {
            var comments = Context.Blog.ToList();

            return Ok(comments);
        }

        [HttpPost]
        public IActionResult CreateComment()
        {
            var comment = new Blog()
            {
                Id=3,
                OwnerId = 1,
                Content = "guzel is",
                LikesCount = 4,
                PostId = 1
            };
            Context.Add(comment);
            Context.SaveChanges();
            return Ok("succesfully created comment");
        }
    }
}