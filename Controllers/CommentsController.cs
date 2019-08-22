using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blog_Project.Dtos;
using Blog_Project.Helpers;
using Blog_Project.Models;
using Blog_Project.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog_Project.Controllers
{
    
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {

        private readonly IRepository<Comment> _commentRepository;

        private readonly IMapper _mapper;

        public CommentsController(IRepository<Comment> commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<List<Comment>> GetAll()
        {
            return _commentRepository.All().ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Comment> Get(string id,
            [FromQuery] bool owner,
            [FromQuery] bool post)
        {

            //Initialize a queryable object for further include operations.
            var commentQueryable = _commentRepository.Where(c => c.Id == Guid.Parse(id));

            //Check if there exists a category with given id
            if (commentQueryable.FirstOrDefault() == null)
            {
                return NotFound(new Message("No such comment with this id: " + id));
            }

            if (owner)
                commentQueryable = commentQueryable.Include(c => c.Owner);

            if (post)
                commentQueryable = commentQueryable.Include(c => c.Post);

            //Get comment object
            var comment = commentQueryable.FirstOrDefault();

            return Ok(comment);
        }

        [HttpPost]
        public ActionResult<Comment> Create([FromBody] CommentCreateDto commentInDto)
        {
            var commentIn = _mapper.Map<Comment>(commentInDto);

            //TODO Authorization with token, check if owner id of the comment equals to the user id in token.

            if (_commentRepository.Add(commentIn))
            {
                return Ok(commentIn);
            }

            return BadRequest(new Message("Error when creating comment"));
        }

        [HttpPost("{id}")]
        public ActionResult<Comment> Update(string id, [FromBody] CommentUpdateDto commentInDto)
        {
            var comment = _commentRepository.GetById(Guid.Parse(id));

            //Check if this comment exists
            if (comment == null)
            {
                return BadRequest(new Message("The comment with id: "+ id + " doesn't exist."));
            }

            //TODO Authorization with token, check if owner id of the comment equals to the user id in token.

            comment.Content = commentInDto.Content;
            comment.LastEditTime = DateTime.Now;

            if (_commentRepository.Update(comment))
            {
                return Ok(comment);
            }

            return BadRequest(new Message("Error when updating comment with id: " + id));

        }

        [HttpPost("{id}")]
        public IActionResult Delete(string id)
        {
            //TODO Authorization with token, check if user id in token equals to the owner id of the comment or owner id of the post

            var commentIn = _commentRepository.GetById(Guid.Parse(id));

            if (commentIn == null)
            {
                return BadRequest(new Message("The comment with id: " + id + " doesn't exist."));
            }

            if (_commentRepository.Delete(commentIn))
            {
                return Ok(new Message("Comment with id: " + id + " deleted successfully"));
            }

            return BadRequest(new Message("Error when deleting comment with id: " + id));

        }

    }
}
