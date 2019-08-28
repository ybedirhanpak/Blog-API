using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blog_Project.Dtos;
using Blog_Project.Dtos.PostDtos;
using Blog_Project.Helpers;
using Blog_Project.Models;
using Blog_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog_Project.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IRepository<Post> _postRepository;

        private readonly IMapper _mapper;

        public PostsController(
            IRepository<Post> postRepository,
            IMapper mapper)
        {
            _postRepository = postRepository;
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
            //Check if token is given by admin
            var tokenUser = HttpContext.User;
            if (AuthorizationHelpers.IsAdmin(tokenUser))
            {
                return Ok(_postRepository.All().Select(p => _mapper.Map<PostOutDto>(p)));
            }

            return Unauthorized(new Message("Unauthorized user."));
        }

        /**
         * Gives response including one post which has given id.
         *
         * GET: api/posts/get/{id}
         */
        [HttpGet("{id}")]
        public ActionResult<PostOutDto> Get(string id,
            [FromQuery] bool owner,
            [FromQuery] bool comment,
            [FromQuery] bool category,
            [FromQuery] bool like)
        {

            //Initialize a queryable object for further include operations.
            var postQueryable = _postRepository.Where(p => p.Id == id);

            var rawPost = postQueryable.FirstOrDefault();

            //Check if there exists a post with given id
            if (rawPost == null)
            {
                return NotFound(new Message("No such post with this id: " + id));
            }

            //Check if token is given by admin or owner of the post
            var tokenUser = HttpContext.User;
            if (!AuthorizationHelpers.IsAdmin(tokenUser) && !AuthorizationHelpers.IsAuthorizedUser(tokenUser, rawPost.OwnerId))
            {
                return Unauthorized(new Message("Unauthorized user."));
            }

            if (owner)
                postQueryable = postQueryable.Include(p => p.Owner);

            if (comment)
                postQueryable = postQueryable.Include(p => p.Comments);

            if (category)
                postQueryable = postQueryable.Include(p => p.Category);

            if (like)
                postQueryable = postQueryable.Include(p => p.LikedUsers).ThenInclude(lu => lu.User);

            //Get the post object
            var post = postQueryable.FirstOrDefault();

            //Find previous post with submit date
            var prevPost = _postRepository.Where(p => p.OwnerId == post.OwnerId && p.SubmitTime < post.SubmitTime)
                .FirstOrDefault();
            var prevPostOutDto = _mapper.Map<PostOutDto>(prevPost);

            //Find next post with submit date
            var nextPost = _postRepository.Where(p => p.OwnerId == post.OwnerId && p.SubmitTime > post.SubmitTime)
                .FirstOrDefault();
            var nextPostOutDto = _mapper.Map<PostOutDto>(nextPost);

            //Prepare post dto
            var postOutDto = _mapper.Map<PostOutDto>(post);
            postOutDto.PreviousPost = prevPostOutDto;
            postOutDto.NextPost = nextPostOutDto;

            return postOutDto;
        }

        /**
         * Gives response including all posts of given owner
         *
         * GET: api/posts/getByOwnerId/{ownerId}
         */
        [HttpGet("{ownerId}")]
        public IActionResult GetByOwnerId(string ownerId,
            [FromQuery] int limit,
            [FromQuery] bool oldest)
        {
            //Check if request is sent by owner of the posts or by admin
            var tokenUser = HttpContext.User;

            if (!AuthorizationHelpers.IsAdmin(tokenUser) && !AuthorizationHelpers.IsAuthorizedUser(tokenUser, ownerId))
            {
                return Unauthorized(new Message("Unauthorized user."));
            }

            //Create queryable object for limitations and order specifications
            var postsQueryable = _postRepository.Where(p => p.OwnerId == ownerId);

            //Order
            postsQueryable = oldest ? postsQueryable.OrderBy(p => p.SubmitTime) : postsQueryable.OrderByDescending(p => p.SubmitTime);

            //Limitation
            if (limit > 0)
                postsQueryable = postsQueryable.Take(limit);

            return Ok(postsQueryable.Select(p => _mapper.Map<PostOutDto>(p)));
        }

        /**
         * Creates new column in table with given post.
         * Gives OK response with post object if adding process is successful.
         *
         * POST: api/posts/create
         */
        [HttpPost]
        public ActionResult<PostOutDto> Create([FromBody] PostCreateDto postInDto)
        {

            if (!ModelState.IsValid || postInDto == null)
                return BadRequest(new Message("Post not valid or null"));

            if(string.IsNullOrEmpty(postInDto.OwnerId))
                return BadRequest(new Message("Please give valid owner Id"));

            if (string.IsNullOrEmpty(postInDto.CategoryId))
                return BadRequest(new Message("Please give valid category Id"));

            if (string.IsNullOrEmpty(postInDto.Content))
                return BadRequest(new Message("Please give valid content"));

            if (string.IsNullOrEmpty(postInDto.Title))
                return BadRequest(new Message("Please give valid title"));

            var postIn = _mapper.Map<Post>(postInDto);

            //Check if post is being created by its owner
            var tokenUser = HttpContext.User;
            if (!AuthorizationHelpers.IsAuthorizedUser(tokenUser, postInDto.OwnerId))
            {
                return Unauthorized(new Message("Unauthorized user."));
            }

            //Update previous post, current post and owner.
            if (!_postRepository.Add(postIn))
                return BadRequest(new Message("Error when adding post into table. Please check owner Id"));

            var postOutDto = _mapper.Map<PostOutDto>(postIn);

            return postOutDto;

        }

        // PUT: api/posts/update
        [HttpPost("{id}")]
        public ActionResult<PostOutDto> Update(string id, [FromBody] PostUpdateDto postInDto)
        {
            if (!ModelState.IsValid || postInDto == null) return BadRequest(new Message("Post not valid or null"));

            //Check if there exist a post with {id}
            var post = _postRepository.GetById(id);
            if (post == null)
                return NotFound(new Message("No such post with this id: " + id));

            //Check if post is being updated by its owner
            var tokenUser = HttpContext.User;
            if (!AuthorizationHelpers.IsAuthorizedUser(tokenUser, post.OwnerId))
            {
                return Unauthorized(new Message("Unauthorized user."));
            }

            if (!string.IsNullOrWhiteSpace(postInDto.Title))
                post.Title = postInDto.Title;

            if (!string.IsNullOrWhiteSpace(postInDto.Content))
                post.Content = postInDto.Content;

            if (!string.IsNullOrWhiteSpace(postInDto.CategoryId))
                post.Content = postInDto.CategoryId;

            //Save changes
            if (!_postRepository.Update(post))
            {
                return BadRequest(new Message("Error when updating post"));
            }

            var postOutDto = _mapper.Map<PostOutDto>(post);

            return postOutDto;
        }

        [AllowAnonymous]
        [HttpPost("{id}")]
        public ActionResult<PostOutDto> IncrementView(string id)
        {
            //Check if there exist a post with {id}
            var post = _postRepository.GetById(id);
            if (post == null)
                return NotFound(new Message("No such post with this id: " + id));

            post.ViewCount += 1;

            //Save changes
            if (!_postRepository.Update(post))
            {
                return BadRequest(new Message("Error when updating post view count"));
            }

            var postOutDto = _mapper.Map<PostOutDto>(post);

            return postOutDto;
        }

        // DELETE: api/posts/delete/5
        [HttpPost("{id}")]
        public IActionResult Delete(string id)
        {
            var post = _postRepository.GetById(id);
            if (post == null)
                return NotFound(new Message("No such post with this id: " + id));

            //Check if post is being deleted by its owner or by admin
            var tokenUser = HttpContext.User;
            if (!AuthorizationHelpers.IsAdmin(tokenUser) && !AuthorizationHelpers.IsAuthorizedUser(tokenUser, post.OwnerId))
            {
                return Unauthorized(new Message("Unauthorized user."));
            }

            //Update table
            if (_postRepository.Delete(post))
                return Ok(new Message("Post with title: "+ post.Title + " and with id: " + post.Id + " deleted Successfully"));

            return BadRequest(new Message("Error when updating post"));
        }
    }
}
