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
        public ActionResult<List<PostOutDto>> GetAll()
        {
            //Check if token is given by admin
            var tokenUser = HttpContext.User;
            if (AuthorizationHelpers.IsAdmin(tokenUser))
            {
                return _postRepository.Where(p => !p.IsDeleted).Select(p => _mapper.Map<PostOutDto>(p)).ToList();
            }

            return BadRequest(new Message("Unauthorized user."));
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
            if (rawPost == null || rawPost.IsDeleted)
            {
                return NotFound(new Message("No such post with this id: " + id));
            }

            //Check if token is given by admin or owner of the post
            var tokenUser = HttpContext.User;
            if (!AuthorizationHelpers.IsAdmin(tokenUser) && !AuthorizationHelpers.IsAuthorizedUser(tokenUser, rawPost.OwnerId))
            {
                return BadRequest(new Message("Unauthorized user."));
            }

            if (owner)
                postQueryable = postQueryable.Include(p => p.Owner);

            if (comment)
                postQueryable = postQueryable.Include(p => p.Comments);

            if (category)
                postQueryable = postQueryable.Include(p => p.RelatedCategories);

            if (like)
                postQueryable = postQueryable.Include(p => p.LikedUsers);

            //Get the post object
            var post = postQueryable.FirstOrDefault();

            //Find previous post with submit date
            var prevPost = _postRepository.Where(p => p.OwnerId == post.OwnerId && p.SubmitDate < post.SubmitDate)
                .FirstOrDefault();
            var prevPostOutDto = _mapper.Map<PostOutDto>(prevPost);

            //Find next post with submit date
            var nextPost = _postRepository.Where(p => p.OwnerId == post.OwnerId && p.SubmitDate > post.SubmitDate)
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
        public ActionResult<List<PostOutDto>> GetByOwnerId(string ownerId,
            [FromQuery] int limit,
            [FromQuery] bool oldest)
        {
            //Check if request is sent by owner of the posts or by admin
            var tokenUser = HttpContext.User;

            if (!AuthorizationHelpers.IsAdmin(tokenUser) && !AuthorizationHelpers.IsAuthorizedUser(tokenUser, ownerId))
            {
                return BadRequest(new Message("Unauthorized user."));
            }

            //Create queryable object for limitations and order specifications
            var postsQueryable = _postRepository.Where(p => p.OwnerId == ownerId && !p.IsDeleted);

            //Order
            postsQueryable = oldest ? postsQueryable.OrderBy(p => p.SubmitDate) : postsQueryable.OrderByDescending(p => p.SubmitDate);

            //Limitation
            if (limit > 0)
                postsQueryable = postsQueryable.Take(limit);

            return postsQueryable.Select(p => _mapper.Map<PostOutDto>(p)).ToList();
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
                return BadRequest(new Message("Unauthorized user."));
            }

            //Update previous post, current post and owner.
            if (!_postRepository.Add(postIn))
                return BadRequest(new Message("Error when adding post into table. Please check owner Id"));
            
            if (!_postCategoryRepository.Add(new PostCategory(postIn.Id, postInDto.CategoryId)))
                return BadRequest(new Message("Error when adding post category into table. Please check category Id"));

            PostOutDto postOutDto = _mapper.Map<PostOutDto>(postIn);

            return postOutDto;

        }

        // PUT: api/posts/update
        [HttpPost("{id}")]
        public ActionResult<PostOutDto> Update(string id, [FromBody] PostUpdateDto postInDto)
        {
            if (!ModelState.IsValid || postInDto == null) return BadRequest(new Message("Post not valid or null"));

            //Check if there exist a post with {id}
            var post = _postRepository.GetById(id);
            if (post == null || post.IsDeleted)
                return NotFound(new Message("No such post with this id: " + id));

            //Check if post is being updated by its owner
            var tokenUser = HttpContext.User;
            if (!AuthorizationHelpers.IsAuthorizedUser(tokenUser, post.OwnerId))
            {
                return BadRequest(new Message("Unauthorized user."));
            }

            //Update post
            post.LastUpdateDate = DateTime.Now;

            var postCategory = _postCategoryRepository.Where(pc => pc.PostId == id).FirstOrDefault();
            var isCategoryUpdated = !string.IsNullOrWhiteSpace(postInDto.CategoryId) && postCategory != null;

            if (isCategoryUpdated)
                postCategory.CategoryId = postInDto.CategoryId;

            if (!string.IsNullOrWhiteSpace(postInDto.Title))
                post.Title = postInDto.Title;

            if (!string.IsNullOrWhiteSpace(postInDto.Content))
                post.Content = postInDto.Content;

            if (postInDto.ViewCount > 0)
                post.ViewCount = postInDto.ViewCount;

            //Save changes
            if (!_postRepository.Update(post))
            {
                return BadRequest(new Message("Error when updating post"));
            }

            //If category is not updated, finish here.
            if (!isCategoryUpdated)
            {
                return Ok(post);
            }

            //Save post-category relation to the table
            if (!_postCategoryRepository.Update(postCategory))
                return BadRequest(new Message("Error when updating post category relation"));

            var postOutDto = _mapper.Map<PostOutDto>(post);

            return postOutDto;
        }

        // DELETE: api/posts/delete/5
        [HttpPost("{id}")]
        public IActionResult Delete(string id)
        {
            var post = _postRepository.GetById(id);
            if (post == null || post.IsDeleted)
                return NotFound(new Message("No such post with this id: " + id));

            //Check if post is being deleted by its owner or by admin
            var tokenUser = HttpContext.User;
            if (!AuthorizationHelpers.IsAdmin(tokenUser) && !AuthorizationHelpers.IsAuthorizedUser(tokenUser, post.OwnerId))
            {
                return BadRequest(new Message("Unauthorized user."));
            }

            //Update post
            post.IsDeleted = true;

            //Update table
            if (_postRepository.Update(post))
                return Ok(new Message("Post with title: "+ post.Title + " and with id: " + post.Id + " deleted Successfully"));

            return BadRequest(new Message("Error when updating post"));
        }
    }
}
