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
using Blog_Project.Settings;

namespace Blog_Project.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserFollow> _userFollowRepository;
        private readonly IRepository<UserLikePost> _userLikePostRepository;
        private readonly IRepository<Post> _postRepository;

        private readonly IMapper _mapper;
        private readonly TokenSettings _tokenSettings;

        public UsersController(IRepository<User> userRepository,IRepository<UserFollow> userFollowRepository,
            IRepository<UserLikePost> userLikePostRepository,
            IRepository<Post> postRepository,
            IMapper mapper,
            IOptions<TokenSettings> tokenSettings)
        {
            this._userRepository = userRepository;
            this._userFollowRepository = userFollowRepository;
            this._userLikePostRepository = userLikePostRepository;
            this._postRepository = postRepository;

            this._mapper = mapper;
            _tokenSettings = tokenSettings.Value;
        }

        // POST api/users/login
        [AllowAnonymous]
        [HttpPost]
        public ActionResult<UserAuthenticatedDto> Login([FromBody]UserLoginDto userInDto)
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

            return userOutDto;
        }

        // POST api/users/register
        [AllowAnonymous]
        [HttpPost]
        public ActionResult<UserOutDto> Register([FromBody]UserCreateDto userInDto)
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
                var userOutDto = _mapper.Map<UserOutDto>(userIn);

                return (userOutDto);
            }

            return BadRequest(new Message("Error when creating user"));
        }

        //GET api/users/get
        [HttpGet]
        public ActionResult<List<UserOutDto>> GetAll()
        {
            //Check if token is given by admin
            var tokenUser = HttpContext.User;
            if (!AuthorizationHelpers.IsAdmin(tokenUser))
            {
                return BadRequest(new Message("Unauthorized user."));
            }

            return _userRepository.Where(u => !u.IsDeleted).Select(u => _mapper.Map<UserOutDto>(u)).ToList();
        }

        //GET api/users/get
        [HttpGet("{id}")]
        public ActionResult<UserOutDto> Get(string id,
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
            var userQueryable = _userRepository.Where(u => u.Id == id);

            var rawUser = userQueryable.FirstOrDefault();

            //Check if there exists a user with given id
            if (rawUser == null || rawUser.IsDeleted)
            {
                return NotFound(new Message("No such user with this id: " + id));
            }

            if (posts)
                userQueryable = userQueryable.Include(u => u.Posts);

            if (likedPosts)
                userQueryable = userQueryable.Include(u => u.LikedPosts);

            if (followings)
                userQueryable = userQueryable.Include(u => u.Followings).ThenInclude(uf => uf.Followed);

            if (followers)
                userQueryable = userQueryable.Include(u => u.Followers).ThenInclude(uf => uf.Follower);

            if (category)
                userQueryable = userQueryable.Include(u => u.InterestedCategories);

            //Get the user object
            var user = userQueryable.FirstOrDefault();

            //Prepare dto object
            var userOutDto = _mapper.Map<UserOutDto>(user);

            return userOutDto;
        }

        // POST api/users/update/id
        [HttpPost("{id}")]
        public ActionResult<UserOutDto> Update(string id, [FromBody]UserUpdateDto userDto)
        {
            var userOld = _userRepository.GetById(id);
            if (userOld == null || userOld.IsDeleted)
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
                var userOutDto = _mapper.Map<UserOutDto>(userOld);

                return userOutDto;
            }

            return BadRequest(new Message("Error when updating user."));
        }

        [HttpPost]
        public IActionResult Follow(UserDoFollowDto userFollowDto)
        {
            //Check if inputs are valid
            if (string.IsNullOrEmpty(userFollowDto.FollowerId))
                return BadRequest(new Message("Please give valid follower id."));

            if (string.IsNullOrEmpty(userFollowDto.FollowedId))
                return BadRequest(new Message("Please give valid followed id."));

            //Check if any of the users is deleted
            if (_userRepository.GetById(userFollowDto.FollowerId).IsDeleted)
            {
                return BadRequest(new Message("Follower user : " + userFollowDto.FollowerId + " is no longer exists"));
            }

            if (_userRepository.GetById(userFollowDto.FollowedId).IsDeleted)
            {
                return BadRequest(new Message("Followed user : " + userFollowDto.FollowedId + " is no longer exists"));
            }

            //Check if token is given by authorized user
            var tokenUser = HttpContext.User;

            //If follower user is not authorized, finish here
            if (!AuthorizationHelpers.IsAuthorizedUser(tokenUser, userFollowDto.FollowerId))
            {
                return BadRequest(new Message("Unauthorized user."));
            }

            //Get follow relation from table
            var userFollow = _userFollowRepository.Where(uf =>
                    uf.FollowerId == userFollowDto.FollowerId && uf.FollowedId == userFollowDto.FollowedId)
                .FirstOrDefault();

            //If such relation exists, finish here.
            if (userFollow != null)
            {
                return BadRequest(new Message("User : " + userFollow.FollowerId + " is already following user : " +
                                              userFollow.FollowedId));
            }

            //Add relation to the table
            if (_userFollowRepository.Add(new UserFollow(userFollowDto.FollowerId, userFollowDto.FollowedId)))
            {
                return Ok(new Message("Follow operation with follower : " + userFollowDto.FollowerId + " and followed : " +
                                      userFollowDto.FollowedId + " is done successfully."));
            }

            return BadRequest(new Message("Error when following user with follower :" +userFollowDto.FollowerId + " and followed : " + userFollowDto.FollowedId));
        }

        [HttpPost]
        public IActionResult UnFollow(UserDoFollowDto userFollowDto)
        {
            //Check if inputs are valid
            if (string.IsNullOrEmpty(userFollowDto.FollowerId))
                return BadRequest(new Message("Please give valid follower id."));

            if (string.IsNullOrEmpty(userFollowDto.FollowedId))
                return BadRequest(new Message("Please give valid followed id."));

            //Check if any of the users is deleted
            if (_userRepository.GetById(userFollowDto.FollowerId).IsDeleted)
            {
                return BadRequest(new Message("Follower user : " + userFollowDto.FollowerId + " is no longer exists"));
            }

            if (_userRepository.GetById(userFollowDto.FollowedId).IsDeleted)
            {
                return BadRequest(new Message("Followed user : " + userFollowDto.FollowedId + " is no longer exists"));
            }

            //Check if token is given by authorized user
            var tokenUser = HttpContext.User;

            //If follower user is not authorized, finish here
            if (!AuthorizationHelpers.IsAuthorizedUser(tokenUser, userFollowDto.FollowerId))
            {
                return BadRequest(new Message("Unauthorized user."));
            }

            //Get follow relation from table
            var userFollow = _userFollowRepository.Where(uf =>
                    uf.FollowerId == userFollowDto.FollowerId && uf.FollowedId == userFollowDto.FollowedId)
                .FirstOrDefault();

            //If such relation doesn't exist , finish here.
            if (userFollow == null)
            {
                return BadRequest(new Message("User : " + userFollowDto.FollowerId + " is not following user : " +
                                              userFollowDto.FollowedId));
            }

            //Delete relation from the table
            if (_userFollowRepository.Delete(userFollow))
            {
                return Ok(new Message("User : " + userFollow.FollowerId + " is not following user : " +
                                      userFollow.FollowedId + " anymore."));
            }

            return BadRequest(new Message("Error when unfollow, with follower :" + userFollowDto.FollowerId + " and followed : " + userFollowDto.FollowedId));
        }

        [HttpPost]
        public IActionResult LikePost(UserLikePostInDto userLikePostDto)
        {
            //Check if inputs are valid
            if (string.IsNullOrEmpty(userLikePostDto.UserId))
                return BadRequest(new Message("Please give valid user id."));

            if (string.IsNullOrEmpty(userLikePostDto.PostId))
                return BadRequest(new Message("Please give valid post id."));

            var userIn = _userRepository.GetById(userLikePostDto.UserId);
            //Check if user is deleted
            if (userIn.IsDeleted)
            {
                return BadRequest(new Message("User : " + userIn.UserName + " is no longer exists"));
            }

            var postIn = _postRepository.GetById(userLikePostDto.PostId);
            //Check if post is deleted
            if (postIn.IsDeleted)
            {
                return BadRequest(new Message("Post : " + postIn.Title + " is no longer exists"));
            }

            //Check if token is given by authorized user
            var tokenUser = HttpContext.User;

            //If follower user is not authorized, finish here
            if (!AuthorizationHelpers.IsAuthorizedUser(tokenUser, userLikePostDto.UserId))
            {
                return BadRequest(new Message("Unauthorized user."));
            }

            //Get the user like relation from table
            var userLikePost = _userLikePostRepository
                .Where(ulp => ulp.UserId == userLikePostDto.UserId && ulp.PostId == userLikePostDto.PostId)
                .FirstOrDefault();

            //Check if there exists such relation
            if (userLikePost != null)
            {
                return BadRequest(new Message("User : " + userLikePostDto.UserId + " is already liked post : " +
                                              userLikePostDto.PostId));
            }

            //Add relation to table
            if (_userLikePostRepository.Add(new UserLikePost(userLikePostDto.UserId, userLikePostDto.PostId)))
            {
                return Ok(new Message("User : " + userLikePostDto.UserId + " is liked post : " +
                                      userLikePostDto.PostId + " successfully"));
            }

            return BadRequest(new Message("Error when post like with user :" + userLikePostDto.UserId+ " and post : " + userLikePostDto.PostId));
        }

        [HttpPost]
        public IActionResult UnlikePost(UserLikePostInDto userLikePostDto)
        {
            //Check if inputs are valid
            if (string.IsNullOrEmpty(userLikePostDto.UserId))
                return BadRequest(new Message("Please give valid user id."));

            if (string.IsNullOrEmpty(userLikePostDto.PostId))
                return BadRequest(new Message("Please give valid post id."));

            var userIn = _userRepository.GetById(userLikePostDto.UserId);
            //Check if user is deleted
            if (userIn.IsDeleted)
            {
                return BadRequest(new Message("User : " + userIn.UserName + " is no longer exists"));
            }

            var postIn = _postRepository.GetById(userLikePostDto.PostId);
            //Check if post is deleted
            if (postIn.IsDeleted)
            {
                return BadRequest(new Message("Post : " + postIn.Title + " is no longer exists"));
            }

            //Check if token is given by authorized user
            var tokenUser = HttpContext.User;

            //If follower user is not authorized, finish here
            if (!AuthorizationHelpers.IsAuthorizedUser(tokenUser, userLikePostDto.UserId))
            {
                return BadRequest(new Message("Unauthorized user."));
            }

            //Get the user like relation from table
            var userLikePost = _userLikePostRepository
                .Where(ulp => ulp.UserId == userLikePostDto.UserId && ulp.PostId == userLikePostDto.PostId)
                .FirstOrDefault();

            //Check if there exists such relation
            if (userLikePost == null)
            {
                return BadRequest(new Message("User : " + userLikePostDto.UserId + " hasn't been liked the post : " +
                                              userLikePostDto.PostId));
            }

            //Delete relation from table
            if (_userLikePostRepository.Delete(userLikePost))
            {
                return Ok(new Message("User : " + userLikePostDto.UserId + " is not being liked the post : " +
                                      userLikePostDto.PostId + " anymore"));
            }

            return BadRequest(new Message("Error when post unlike with user :" + userLikePostDto.UserId + " and post : " + userLikePostDto.PostId));
        }

        // POST api/users/delete/id
        [HttpPost("{id}")]
        public IActionResult Delete(string id)
        {
            //Check if token is given by admin or authorized user
            var tokenUser = HttpContext.User;

            if (!AuthorizationHelpers.IsAdmin(tokenUser) && !AuthorizationHelpers.IsAuthorizedUser(tokenUser, id))
            {
                return BadRequest(new Message("Unauthorized user."));
            }

            //Find user in table
            var user = _userRepository.Where(u => u.Id == id).Include(u => u.Posts).FirstOrDefault();

            //Check if such user exists
            if (user == null)
            {
                return BadRequest(new Message("No such user with this id: " + id));
            }

            //Update user
            user.IsDeleted = true;

            //Update user's posts
            foreach (var p in user.Posts)
            {
                p.IsDeleted = true;
                _postRepository.Update(p);
            }

            if (_userRepository.Update(user))
            {
                return Ok(new Message("User with email: " + user.Email + ", id: " + id + " deleted successfully"));
            }

            return BadRequest(new Message("Error when deleting user with id: "+ id));

        }

    }
}