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
using Blog_Project.Dtos.UserDtos;

namespace Blog_Project.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly TokenSettings _tokenSettings;

        public UsersController(IRepository<User> userRepository, IMapper mapper, IOptions<TokenSettings> tokenSettings)
        {
            this._userRepository = userRepository;
            this._mapper = mapper;
            _tokenSettings = tokenSettings.Value;
        }

        // POST api/users/login
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody]UserLoginDto userInDto)
        {
            if ((string.IsNullOrEmpty(userInDto.Email) && string.IsNullOrEmpty(userInDto.UserName)) || string.IsNullOrEmpty(userInDto.Password))
            {
                return BadRequest(new Message("Username (and Email) or Password is empty."));
            }

            User user = null;

            if (!string.IsNullOrEmpty(userInDto.Email) && !string.IsNullOrEmpty(userInDto.UserName))
                user = _userRepository.Where(u => u.Email == userInDto.Email && u.UserName == userInDto.UserName).FirstOrDefault();
            else if (!string.IsNullOrEmpty(userInDto.Email))
                user = _userRepository.Where(u => u.Email == userInDto.Email).FirstOrDefault();
            else if(!string.IsNullOrEmpty(userInDto.UserName))
                user = _userRepository.Where(u => u.UserName == userInDto.UserName).FirstOrDefault();

            // Check if user exists
            if (user == null)
                return BadRequest(new Message("User not found. Please check your email and/or username."));

            if (!UserHelpers.VerifyPasswordHash(userInDto.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest(new Message("Password is incorrect."));
            }

            var userOutDto = _mapper.Map<UserAuthenticatedDto>(user);
            userOutDto.Token = UserHelpers.GenerateToken(user, _tokenSettings);

            return Ok(userOutDto);
        }

        // POST api/users/register
        [AllowAnonymous]
        [HttpPost]
        public ActionResult<User> Register([FromBody]UserCreateDto userInDto)
        {
            //Check if given email is already used.
            if (_userRepository.Where(u => u.Email == userInDto.Email).Any())
                return BadRequest(new Message("Email: " + userInDto.Email + " already exists."));

            //Check if given username is already used.
            if (_userRepository.Where(u => u.UserName == userInDto.UserName).Any())
                return BadRequest(new Message("Username: " + userInDto.UserName + " already exists."));

            //Check if password is valid.
            if (string.IsNullOrWhiteSpace(userInDto.Email))
                return BadRequest(new Message("Email is invalid."));

            //Check if password is valid.
            if (string.IsNullOrWhiteSpace(userInDto.UserName))
                return BadRequest(new Message("Username is invalid."));

            //Check if password is valid.
            if (string.IsNullOrWhiteSpace(userInDto.Password))
                return BadRequest(new Message("Password is invalid."));



            UserHelpers.CreatePasswordHash(userInDto.Password, out var passwordHash, out var passwordSalt);

            var userIn = _mapper.Map<User>(userInDto);

            //Save user's password
            userIn.PasswordHash = passwordHash;
            userIn.PasswordSalt = passwordSalt;

            //Set user's role "User" by default.
            userIn.Role = Role.User;

            //Save user to the table
            if (_userRepository.Add(userIn))
            {
                return (userIn);
            }

            return BadRequest(new Message("Error when creating user"));
        }

        //GET api/users/get
        [HttpGet]
        public ActionResult<List<User>> GetAll()
        {
            //Check if token is given by admin
            var tokenUser = HttpContext.User;
            if (AuthorizationHelpers.IsAdmin(tokenUser))
            {
                return Ok(_userRepository.All().ToList());
            }

            return BadRequest(new Message("Unauthorized user."));
        }

        //GET api/users/get
        [HttpGet("{id}")]
        public ActionResult<User> Get(string id,
            [FromQuery] bool posts,
            [FromQuery] bool likedPosts,
            [FromQuery] bool followings,
            [FromQuery] bool followers,
            [FromQuery] bool category)
        {
            //Check if token is given by admin or authorized user
            var tokenUser = HttpContext.User;

            if (!AuthorizationHelpers.IsAdmin(tokenUser) && !AuthorizationHelpers.IsAuthorizedUser(tokenUser, id))
            {
                return BadRequest(new Message("Unauthorized user."));
            }

            //Initialize a queryable object for further include operations.
            var userQueryable = _userRepository.Where(u => u.Id == Guid.Parse(id));

            //Check if there exists a user with given id
            if (userQueryable.FirstOrDefault() == null)
            {
                return NotFound(new Message("No such user with this id: " + id));
            }

            if (posts)
                userQueryable = userQueryable.Include(u => u.Posts);

            if (likedPosts)
                userQueryable = userQueryable.Include(u => u.LikedPosts);

            if (followings)
                userQueryable = userQueryable.Include(u => u.Followings);

            if (followers)
                userQueryable = userQueryable.Include(u => u.Followers);

            if (category)
                userQueryable = userQueryable.Include(u => u.InterestedCategories);

            //Get the user object
            var user = userQueryable.FirstOrDefault();

            //Prepare dto object
            var userOutDto = _mapper.Map<UserOutDto>(user);

            return Ok(userOutDto);
        }

        // POST api/users/update/id
        [HttpPost("{id}")]
        public ActionResult<User> Update(string id, [FromBody]UserUpdateDto userDto)
        {
            var userOld = _userRepository.GetById(Guid.Parse(id));
            if (userOld == null)
            {
                return NotFound(new Message("No such user with this id: " + id));
            }

            //Check if token is given by admin or authorized user
            var tokenUser = HttpContext.User;

            if (!AuthorizationHelpers.IsAdmin(tokenUser) && !AuthorizationHelpers.IsAuthorizedUser(tokenUser,id))
            {
                return BadRequest(new Message("Unauthorized user."));
            }

            if (!string.IsNullOrWhiteSpace(userDto.BirthDate))
                userOld.BirthDate = userDto.BirthDate;

            if (!string.IsNullOrWhiteSpace(userDto.Description))
                userOld.Description = userDto.Description;

            if (!string.IsNullOrWhiteSpace(userDto.FacebookLink))
                userOld.FacebookLink = userDto.FacebookLink;

            if (!string.IsNullOrWhiteSpace(userDto.InstagramLink))
                userOld.InstagramLink = userDto.InstagramLink;

            if (!string.IsNullOrWhiteSpace(userDto.LinkedinLink))
                userOld.LinkedinLink = userDto.LinkedinLink;

            if (!string.IsNullOrWhiteSpace(userDto.Theme))
                userOld.Theme = userDto.Theme;

            if (!string.IsNullOrWhiteSpace(userDto.TwitterLink))
                userOld.TwitterLink = userDto.TwitterLink;

            if (!string.IsNullOrWhiteSpace(userDto.Password))
            {
                UserHelpers.CreatePasswordHash(userDto.Password,out var passwordHash, out var passwordSalt);
                userOld.PasswordHash = passwordHash;
                userOld.PasswordSalt = passwordSalt;
            }

            if (_userRepository.Update(userOld))
            {
                return Ok(userOld);
            }

            return BadRequest(new Message("Error when updating user."));
        }

        // POST api/users/delete/id
        [HttpPost("{id}")]
        public ActionResult<User> Delete(string id)
        {
            //Check if token is given by admin or authorized user
            var tokenUser = HttpContext.User;

            if (!AuthorizationHelpers.IsAdmin(tokenUser) && !AuthorizationHelpers.IsAuthorizedUser(tokenUser, id))
            {
                return BadRequest(new Message("Unauthorized user."));
            }

            var userIn = _userRepository.GetById(Guid.Parse(id));

            if (userIn == null)
            {
                return BadRequest(new Message("No such user with this id: " + id));
            }

            if (_userRepository.Delete(userIn))
            {
                return Ok(new Message("User with email: " + userIn.Email + ", id: " + id + " deleted successfully"));
            }

            return BadRequest(new Message("Error when deleting user."));

        }

    }
}