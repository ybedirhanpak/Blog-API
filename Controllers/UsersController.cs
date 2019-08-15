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
    public class UsersController : ControllerBase
    {
        private readonly IRepository<User> _userRepository;

        private readonly IMapper _mapper;

        public UsersController(IRepository<User> userRepository, IMapper mapper)
        {
            this._userRepository = userRepository;
            this._mapper = mapper;
        }

        //GET api/users/get
        [HttpGet]
        public ActionResult<List<User>> GetAll()
        {
            return _userRepository.All().Include(u => u.Followers).Include(u => u.Posts).OrderBy(u => u.UserName).ToList();
        }

        //GET api/users/getById
        [HttpGet("{id}", Name = "GetCategory")]
        public ActionResult<User> Get(string id)
        {
            var user = _userRepository.GetById(Guid.Parse(id));
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // POST api/users/create
        [HttpPost]
        public ActionResult<User> Create([FromBody]UserInDto userDto)
        {
            
            var userIn = _mapper.Map<User>(userDto);

            if (_userRepository.Add(userIn))
            {
                return (userIn);
            }
            return BadRequest();

        }

        // POST api/users/update/id
        [HttpPost("{id}")]
        public ActionResult<User> Update(string id,[FromBody]UserInDto userDto)
        {
            var userOld = _userRepository.GetById(Guid.Parse(id));
            if (userOld == null)
            {
                return NotFound();
            }

            var userIn = _mapper.Map<User>(userDto);
            userIn.Id = Guid.Parse(id);

            if (_userRepository.Update(userIn))
            {
                return (userIn);
            }
            return BadRequest();
        }

        // POST api/users/delete/id
        [HttpPost("{id}")]
        public ActionResult<User> Delete(string id)
        {

            var userIn = _userRepository.GetById(Guid.Parse(id));

            if (userIn != null && _userRepository.Delete(userIn))
            {
                return Ok();
            }
            return BadRequest();

        }
    }
}
