using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blog_Project.Dtos;
using Blog_Project.Helpers;
using Blog_Project.Models;
using Blog_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Blog_Project.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class UsersController : ControllerBase
    {
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UsersController(IRepository<User> userRepository, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            this._userRepository = userRepository;
            this._mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
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

            return Ok(new
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.UserName,
                Token = UserHelpers.GenerateToken(user, _appSettings)

            });
        }

        //GET api/users/get
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<List<User>> GetAll()
        {
            var x = Ok(_userRepository.All().ToList());
            return x;
        }

        //GET api/users/get
        [HttpGet]
        public ActionResult<List<User>> GetAllWithPosts()
        {
            var x = Ok(_userRepository.All().Include(u => u.Posts).ToList());
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
        [AllowAnonymous]
        [HttpPost]
        public ActionResult<User> Create([FromBody]UserInDto userDto)
        {
            byte[] passwordHash, passwordSalt;
            UserHelpers.CreatePasswordHash(userDto.Password, out passwordHash, out passwordSalt);

            var userIn = _mapper.Map<User>(userDto);

            userIn.PasswordHash = passwordHash;
            userIn.PasswordSalt = passwordSalt;

            userIn.Role = "User";

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
        [AllowAnonymous]
        [HttpPost("{id}")]
        public ActionResult<User> Update(string id, [FromBody]UserInDto userDto)
        {
            var userOld = _userRepository.GetById(Guid.Parse(id));
            if (userOld == null)
            {
                return NotFound();
            }

            var userIn = _mapper.Map<User>(userDto);
            userIn.Id = Guid.Parse(id);

            userIn.PasswordHash = userOld.PasswordHash;
            userIn.PasswordSalt = userOld.PasswordSalt;

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
        [HttpGet("{id}")]
        public ActionResult<User> Deneme(string id)
        {

            var userIn = _userRepository.GetById(Guid.Parse(id));

            var currentUser = HttpContext.User;
            int spendingTimeWithCompany = 0;
                
            

            if (true)
            {
                return Ok(currentUser);
            }

            return BadRequest();

        }
    }
}