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
        private readonly IRepository<Post> _postRepository;
        private readonly IRepository<PostCategory> _postCategoryRepository;

        private readonly IMapper _mapper;

        public PostsController(
            IRepository<Post> postRepository,
            IRepository<PostCategory> postCategoryRepository, 
            IMapper mapper)
        {
            _postRepository = postRepository;
            _postCategoryRepository = postCategoryRepository;
            _mapper = mapper;
        }

        /**
         * Gives response including all posts in table.
         *
         * GET: api/posts/getAll
         */
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_postRepository.All());
        }

        /**
         * Gives response including one post which has given id.
         *
         * GET: api/posts/get/{id}
         */
        [HttpGet("{id}")]
        public ActionResult<Post> Get(string id)
        {
            var post = _postRepository.GetById(Guid.Parse(id));

            if (post == null)
            {
                return NotFound("No such post with this id: "+id);
            }

            var prevPost = _postRepository.Where(p => p.OwnerId == post.OwnerId && p.SubmitDate < post.SubmitDate)
                .FirstOrDefault();

            var nextPost = _postRepository.Where(p => p.OwnerId == post.OwnerId && p.SubmitDate > post.SubmitDate)
                .FirstOrDefault();

            var postOutDto = _mapper.Map<PostOutDto>(post);
            postOutDto.PreviousPost = prevPost;
            postOutDto.NextPost = nextPost;

            return Ok(postOutDto);
        }

        /**
         * Creates new column in table with given post.
         * Gives OK response with post object if adding process is successful.
         *
         * POST: api/posts/create
         */
        [HttpPost]
        public IActionResult Create([FromBody] PostInDto postInDto)
        {

            if (!ModelState.IsValid || postInDto == null) return BadRequest(error:"Post not valid or null");

            var postIn = _mapper.Map<Post>(postInDto);

            //Update previous post, current post and owner.
            if (!_postRepository.Add(postIn))
                return BadRequest(error: "Error when adding post into table. Please check owner Id");
            
            if (!_postCategoryRepository.Add(new PostCategory(postIn.Id, postInDto.CategoryId)))
                return BadRequest(error: "Error when adding post category into table. Please check category Id");

            return Ok(postIn);

        }

        // PUT: api/posts/update
        [HttpPost("{id}")]
        public IActionResult Update(string id, [FromBody] Post postIn)
        {
            if (!ModelState.IsValid || postIn == null) return BadRequest(error:"Post not valid or null");

            //Check if there exist a post with {id}
            var post = _postRepository.GetById(Guid.Parse(id));
            if (post == null) return NotFound("No such post with this id: "+id);

            //Update post
            postIn.Id = post.Id;
            postIn.LastUpdateDate = DateTime.Now;

            if (_postRepository.Update(postIn))
                return Ok(postIn);

            return BadRequest(error:"Error when updating post");
        }

        // DELETE: api/posts/delete/5
        [HttpPost("{id}")]
        public IActionResult Delete(string id)
        {
            var post = _postRepository.GetById(Guid.Parse(id));
            if (post == null) return NotFound("No such post with this id: " + id);

            if (_postRepository.Delete(post))
                return Ok("Post Deleted Successfully");

            return BadRequest(error: "Error when updating post");
        }
    }
}
