using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog_Project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog_Project.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommentController : ControllerBase
    {

        readonly CommentContext Context;

        public CommentController(CommentContext context )
        {
            Context = context;
        }

        // GET: api/Comment
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Comment/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Comment
        [HttpPost]
        public void Post([FromBody] string value)
        {

        }
        [HttpGet]
        public IActionResult GetComment()
        {
            var comments = Context.Comment.ToList();

            return Ok(comments);
        }

        [HttpPost]
        public IActionResult CreateComment()
        {
            var comment = new Comment()
            {
                OwnerId = "21321421",
                Content = "guzel is",
                LikeCount = 4,
                PostId = "12321321321"
            };
            Context.Add(comment);
            Context.SaveChanges();
            return Ok("succesfully created comment");
        }

        // PUT: api/Comment/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
