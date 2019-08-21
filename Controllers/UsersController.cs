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
    public class UsersController : ControllerBase
    {
        private readonly IRepository<User> _userRepository;

        private readonly IMapper _mapper;

        public UsersController(IRepository<User> userRepository, IMapper mapper)
        {
            this._userRepository = userRepository;
            this._mapper = mapper;
        }

        [HttpPost]
        public IActionResult Authenticate([FromBody]UserInDto userDto)
        {
            if (string.IsNullOrEmpty(userDto.Email) || string.IsNullOrEmpty(userDto.Password))
            {
                return null;
            }

            var user = _userRepository.Where(u => u.Email == userDto.Email).FirstOrDefault();

            // return null if user not found
            if (user == null)
                return null;

            if (!UserHelpers.VerifyPasswordHash(userDto.Password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            return Ok("Login Successful");
        }
        
        //GET api/users/get
        [HttpGet]
        public ActionResult<List<User>> GetAll()
        {
            var x = Ok(_userRepository.All().ToList());
            return x;
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
            byte[] passwordHash, passwordSalt;
            UserHelpers.CreatePasswordHash(userDto.Password, out passwordHash, out passwordSalt);

            var userIn = _mapper.Map<User>(userDto);

            userIn.PasswordHash = passwordHash;
            userIn.PasswordSalt = passwordSalt;

            if (string.IsNullOrWhiteSpace(userDto.Password))
            {
                throw new AppException("User not found");
            }

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
