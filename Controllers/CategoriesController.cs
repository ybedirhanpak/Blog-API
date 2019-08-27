using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blog_Project.Dtos;
using Blog_Project.Dtos.CategoryDtos;
using Blog_Project.Helpers;
using Blog_Project.Models;
using Blog_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog_Project.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<UserCategory> _userCategoryRepository;
        private readonly IRepository<MainCategory> _mainCategoryRepository;
        private readonly IRepository<User> _userRepository;

        private readonly IMapper _mapper;

        public CategoriesController(IRepository<Category> categoryRepository,IRepository<User> userRepository, IRepository<UserCategory> userCategoryRepository, IRepository<MainCategory> mainCategoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _userCategoryRepository = userCategoryRepository;
            _userRepository = userRepository;
            _mainCategoryRepository = mainCategoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            //Check if request is sent by admin
            var tokenUser = HttpContext.User;

            //Check if request is sent by admin.
            if (!AuthorizationHelpers.IsAdmin(tokenUser))
            {
                return Unauthorized(new Message("Unauthorized user."));
            }

            return Ok(_categoryRepository.All().OrderBy(c => c.Name).Select(c => _mapper.Map<CategoryOutDto>(c)).OrderBy(c => c.Name));
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public ActionResult<CategoryOutDto> Get(string id,
            [FromQuery] bool parent,
            [FromQuery] bool post,
            [FromQuery] bool user)
        {

            //Initialize a queryable object for further include operations.
            var categoryQueryable = _categoryRepository.Where(c => c.Id == id);

            var categoryRaw = categoryQueryable.FirstOrDefault();
            //Check if there exists a category with given id
            if (categoryRaw == null)
            {
                return NotFound(new Message("No such category with this id: " + id));
            }

            if (parent)
                categoryQueryable = categoryQueryable.Include(c => c.Parent);

            if (post)
                categoryQueryable = categoryQueryable.Include(c => c.RelatedPosts);

            if (user)
                categoryQueryable = categoryQueryable.Include(c => c.FollowerUsers).ThenInclude(fu => fu.User);

            //Get the category object
            var category = categoryQueryable.FirstOrDefault();

            var categoryOutDto = _mapper.Map<CategoryOutDto>(category);

            return Ok(categoryOutDto);
        }

        [HttpPost]
        public ActionResult<CategoryOutDto> Create([FromBody] CategoryInDto categoryInDto)
        {
            //Check if inputs are valid
            if (string.IsNullOrEmpty(categoryInDto.ImageUrl))
                return BadRequest(new Message("Please give valid image Url"));

            if (string.IsNullOrEmpty(categoryInDto.Name))
                return BadRequest(new Message("Please give valid name"));

            if (string.IsNullOrEmpty(categoryInDto.ParentId))
                return BadRequest(new Message("Please give valid parent Id"));

            //Check if request is sent by admin.
            if (!AuthorizationHelpers.IsAdmin(HttpContext.User))
            {
                return Unauthorized(new Message("Unauthorized user."));
            }

            if (_mainCategoryRepository.GetById(categoryInDto.ParentId) == null)
            {
                return BadRequest(new Message("There is no main category with id: " + categoryInDto.ParentId));
            }

            var categoryIn = _mapper.Map<Category>(categoryInDto);

            if (_categoryRepository.Add(categoryIn))
            {
                var categoryOutDto = _mapper.Map<CategoryOutDto>(categoryIn);

                return Ok(categoryOutDto);
            }

            return BadRequest(new Message("Error when creating category"));
        }

        [HttpPost]
        public ActionResult<UserCategoryOutDto> AddUser([FromBody] UserCategoryInDto userCategoryDto)
        {
            //Check if inputs are valid
            if (string.IsNullOrEmpty(userCategoryDto.UserId))
                return BadRequest(new Message("Please give valid user id"));

            if (string.IsNullOrEmpty(userCategoryDto.CategoryId))
                return BadRequest(new Message("Please give valid category id"));
            
            //Check if user is deleted
            var userIn = _userRepository.GetById(userCategoryDto.UserId);
            if (userIn == null)
                return BadRequest(new Message("User: " + userCategoryDto.UserId + " no longer exists"));

            //Check if category is deleted
            var categoryIn = _categoryRepository.GetById(userCategoryDto.CategoryId);
            if (categoryIn == null)
                return BadRequest(new Message("Category: " + userCategoryDto.CategoryId + " no longer exists"));

            var tokenUser = HttpContext.User;
            //Check if request is sent by user (who is being follower of the category) .
            if (!AuthorizationHelpers.IsAuthorizedUser(tokenUser, userCategoryDto.UserId))
            {
                return Unauthorized(new Message("Unauthorized user."));
            }

            //Get user-category relation from table
            var userCategory = _userCategoryRepository
                .Where(uc => uc.CategoryId == userCategoryDto.CategoryId && uc.UserId == userCategoryDto.UserId)
                .FirstOrDefault();

            //If such relation exists
            if (userCategory != null)
            {
                return BadRequest(new Message("User : " + userCategoryDto.UserId + " is already following Category : " + userCategoryDto.CategoryId));
            }

            //Create new relation
            var userCategoryIn = new UserCategory(userCategoryDto.UserId, userCategoryDto.CategoryId);

            //Update table
            if (_userCategoryRepository.Add(userCategoryIn))
            {
                var userCategoryOutDto = _mapper.Map<UserCategoryOutDto>(userCategoryIn);

                return Ok(userCategoryOutDto);
            }

            return BadRequest(new Message("Error when adding user-category relation"));
        }

        [HttpPost]
        public IActionResult RemoveUser([FromBody] UserCategoryInDto userCategoryDto)
        {
            //Check if inputs are valid
            if (string.IsNullOrEmpty(userCategoryDto.UserId))
                return BadRequest(new Message("Please give valid user id"));

            if (string.IsNullOrEmpty(userCategoryDto.CategoryId))
                return BadRequest(new Message("Please give valid category id"));

            //Check if user is deleted
            var userIn = _userRepository.GetById(userCategoryDto.UserId);
            if (userIn == null)
                return BadRequest(new Message("User: " + userCategoryDto.UserId + " no longer exists"));

            //Check if category is deleted
            var categoryIn = _categoryRepository.GetById(userCategoryDto.CategoryId);
            if (categoryIn == null)
                return BadRequest(new Message("Category: " + userCategoryDto.CategoryId + " no longer exists"));

            var tokenUser = HttpContext.User;
            //Check if request is sent by user (follower of the category) .
            if (!AuthorizationHelpers.IsAuthorizedUser(tokenUser, userCategoryDto.UserId))
            {
                return Unauthorized(new Message("Unauthorized user."));
            }

            //Get user-category relation from table
            var userCategory = _userCategoryRepository
                .Where(uc => uc.CategoryId == userCategoryDto.CategoryId && uc.UserId == userCategoryDto.UserId)
                .FirstOrDefault();

            //If such relation doesn't exist
            if (userCategory == null)
            {
                return BadRequest(new Message("User : " + userCategoryDto.UserId + " is not following Category : " + userCategoryDto.CategoryId));
            }

            //Update table
            if (_userCategoryRepository.Delete(userCategory))
            {
                return Ok(new Message("User : " + userCategory.UserId + " is deleted from Category : " + userCategory.CategoryId));
            }

            return BadRequest(new Message("Error when deleting user-category relation"));
        }


        [HttpPost("{id}")]
        public ActionResult<CategoryOutDto> Update(string id, [FromBody] CategoryInDto categoryInDto)
        {
            //Check if request is sent by admin.
            if (!AuthorizationHelpers.IsAdmin(HttpContext.User))
            {
                return Unauthorized(new Message("Unauthorized user."));
            }

            //Check if there exists a category with given id
            var category = _categoryRepository.GetById(id);
            if (category == null)
            {
                return NotFound(new Message("No such category with this id: " + id));
            }

            if (!string.IsNullOrWhiteSpace(categoryInDto.ImageUrl))
                category.ImageUrl = categoryInDto.ImageUrl;

            if (!string.IsNullOrWhiteSpace(categoryInDto.Name))
                category.Name = categoryInDto.Name;

            if (!string.IsNullOrWhiteSpace(categoryInDto.ParentId))
                category.ParentId = categoryInDto.ParentId;

            //Update category
            if (_categoryRepository.Update(category))
            {
                var categoryOutDto = _mapper.Map<CategoryOutDto>(category);
                return Ok(categoryOutDto);
            }

            return BadRequest(new Message("Error when updating category"));
        }

        // POST api/users/delete/id
        [HttpPost("{id}")]
        public IActionResult Delete(string id)
        {

            //Check if request is sent by admin.
            if (!AuthorizationHelpers.IsAdmin(HttpContext.User))
            {
                return Unauthorized(new Message("Unauthorized user."));
            }

            var category = _categoryRepository.GetById(id);

            //Check if there exists a category with given id
            if (category == null)
            {
                return NotFound(new Message("No such category with this id: " + id));
            }

            //Update table
            if (_categoryRepository.Delete(category))
            {
                return Ok(new Message("Category with name: "+ category.Name +" and id: "+ category.Id +"Deleted Successfully"));
            }

            return BadRequest(new Message("Error when deleting category with id: "+ id));
        }

    }
}
