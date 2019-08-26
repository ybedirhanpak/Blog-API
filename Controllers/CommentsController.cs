using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blog_Project.Dtos.CommentDtos;
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
        public ActionResult<List<CommentOutDto>> GetAll()
        {
            //If request is not sent by admin, finish here
            if (!AuthorizationHelpers.IsAdmin(HttpContext.User))
            {
                return BadRequest(new Message("Unauthorized user."));
            }

            return _commentRepository.Where(c => !c.IsDeleted).Select(c => _mapper.Map<CommentOutDto>(c)).ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<CommentOutDto> Get(string id,
            [FromQuery] bool owner,
            [FromQuery] bool post)
        {

            //If request is not sent by admin, finish here
            if (!AuthorizationHelpers.IsAdmin(HttpContext.User))
                return BadRequest(new Message("Unauthorized user."));


            //Initialize a queryable object for further include operations.
            var commentQueryable = _commentRepository.Where(c => c.Id == id);

            var commentRaw = commentQueryable.FirstOrDefault();
            //Check if there exists a category with given id
            if (commentRaw == null || commentRaw.IsDeleted)
                return NotFound(new Message("No such comment with this id: " + id));

            if (owner)
                commentQueryable = commentQueryable.Include(c => c.Owner);

            if (post)
                commentQueryable = commentQueryable.Include(c => c.Post);

            //Get comment object
            var comment = commentQueryable.FirstOrDefault();

            var commentOutDto = _mapper.Map<CommentOutDto>(comment);

            return Ok(commentOutDto);
        }

        [HttpPost]
        public ActionResult<CommentOutDto> Create([FromBody] CommentCreateDto commentInDto)
        {

            var tokenUser = HttpContext.User;
            //If request is not sent by owner, finish here
            if (!AuthorizationHelpers.IsAuthorizedUser(tokenUser, commentInDto.OwnerId))
                return BadRequest(new Message("Unauthorized user."));
            
            var commentIn = _mapper.Map<Comment>(commentInDto);

            if (_commentRepository.Add(commentIn))
            {
                var commentOutDto = _mapper.Map<CommentOutDto>(commentIn);
                return Ok(commentOutDto);
            }

            return BadRequest(new Message("Error when creating comment"));
        }

        [HttpPost("{id}")]
        public ActionResult<CommentOutDto> Update(string id, [FromBody] CommentUpdateDto commentInDto)
        {
            var comment = _commentRepository.GetById(id);

            //Check if this comment exists
            if (comment == null || comment.IsDeleted)
            {
                return BadRequest(new Message("The comment with id: "+ id + " doesn't exist."));
            }

            var tokenUser = HttpContext.User;
            //If request is not sent by owner, finish here
            if (!AuthorizationHelpers.IsAuthorizedUser(tokenUser, comment.OwnerId))
            {
                return BadRequest(new Message("Unauthorized user."));
            }

            //Update comment content
            comment.LastEditTime = DateTime.Now;

            if (!string.IsNullOrEmpty(commentInDto.Content))
                comment.Content = commentInDto.Content;

            //Update table
            if (_commentRepository.Update(comment))
            {
                var commentOutDto = _mapper.Map<CommentOutDto>(comment);

                return Ok(commentOutDto);
            }

            return BadRequest(new Message("Error when updating comment with id: " + id));

        }

        [HttpPost("{id}")]
        public IActionResult Delete(string id)
        {
            //Get comment from table
            var comment = _commentRepository.GetById(id);

            //Check if such comment exists
            if (comment == null || comment.IsDeleted)
            {
                return BadRequest(new Message("The comment with id: " + id + " doesn't exist."));
            }

            var tokenUser = HttpContext.User;
            //If request is not sent by owner or by admin, finish here
            if (!AuthorizationHelpers.IsAuthorizedUser(tokenUser, comment.OwnerId) && !AuthorizationHelpers.IsAdmin(tokenUser))
                return BadRequest(new Message("Unauthorized user."));

            //Update comment
            comment.IsDeleted = true;

            //Update table
            if (_commentRepository.Update(comment))
            {
                return Ok(new Message("Comment with id: " + id + " deleted successfully"));
            }

            return BadRequest(new Message("Error when deleting comment with id: " + id));

        }

    }
}
