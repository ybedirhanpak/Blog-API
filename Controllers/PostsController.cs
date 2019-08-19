using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blog_Project.Dtos;
using Blog_Project.Models;
using Blog_Project.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog_Project.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IRepository<Post> postRepository;

        public PostsController(IRepository<Post> postRepository)
        {
            this.postRepository = postRepository;
        }
        // GET: api/Post
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(postRepository.All());
        }
        [HttpGet]
        public ActionResult<List<Post>> GetAll()
        {
            List<Post> list =  postRepository.All().Include(p => p.NextPost).Include(p => p.PreviousPost).ToList();
            return list;
        }

        // GET: api/Post/5
        [HttpGet("{id}")]
        public ActionResult<Post> Get(string id)
        {
            var user = postRepository.GetById(Guid.Parse(id));
            if (user == null)
            {
                return NotFound();
            }

            return user;
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

            Post post1 = new Post();

            post1 = post;

            if (postRepository.Add(post1))
                return Ok(post1);

            return BadRequest();
        }

        // PUT: api/Post/5
        [HttpPost("{sId}")]
        public IActionResult Update(string sId, [FromBody] Post post)
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
        [HttpPost("{sId}")]
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
