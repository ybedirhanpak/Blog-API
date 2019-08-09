using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Blog_Project.Services;
using Blog_Project.Models;
using Blog_Project.Settings;

namespace Blog_Project.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IRepository<Post> postRepository;

        public PostController(BlogDbContext dbContext)
        {
            this.postRepository = new Repository<Post>(dbContext);
        }
        // GET: api/Post
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(postRepository.All());
        }

        // GET: api/Post/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok();
        }

        // POST: api/Post
        [HttpPost]
        public IActionResult Post([FromBody] Post post)
        {
            /* if (!ModelState.IsValid || post == null) return BadRequest();

             if (postRepository.Add(post))
                 return Ok(post);

             return BadRequest();*/

            if (!ModelState.IsValid || post == null) return BadRequest();

            var post1 = new Post();

            post1 = post;

            if (postRepository.Add(post))
                return Ok(post);

            return BadRequest();
        }

        // PUT: api/Post/5
        [HttpPut("{id}")]
        public IActionResult Put(string sId, [FromBody] Post post)
        {
            Guid id = Guid.Parse(sId);

            if (!ModelState.IsValid || post == null) return BadRequest();

            var oldPost = postRepository.GetById(id);
            if (oldPost == null) return NotFound();

            post.Id = oldPost.Id;
            if (postRepository.Update(post))
                return Ok(post);
            return BadRequest();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string sId)
        {
            Guid id = Guid.Parse(sId);

            var post = postRepository.GetById(id);
            if (post == null) return NotFound();

            if (postRepository.Delete(post))
                return Ok(post);
            return BadRequest();
        }
    }
}
