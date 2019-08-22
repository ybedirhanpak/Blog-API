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
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IRepository<Category> _categoryRepository;

        private readonly IRepository<UserCategory> _userCategoryRepository;

        private readonly IMapper _mapper;

        public CategoriesController(IRepository<Category> categoryRepository, IRepository<UserCategory> userCategoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _userCategoryRepository = userCategoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<List<Category>> GetAll()
        {
            return _categoryRepository.All().OrderBy(c => c.Name).ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Category> Get(string id,
            [FromQuery] bool parent, 
            [FromQuery] bool children,
            [FromQuery] bool post,
            [FromQuery] bool user)
        {
            //Initialize a queryable object for further include operations.
            var categoryQueryable = _categoryRepository.Where(c => c.Id == Guid.Parse(id));

            //Check if there exists a category with given id
            if (categoryQueryable.FirstOrDefault() == null)
            {
                return NotFound("No such category with this id: " + id);
            }

            if (parent)
                categoryQueryable = categoryQueryable.Include(c => c.Parent);

            if (children)
                categoryQueryable = categoryQueryable.Include(c => c.Children);

            if (post)
                categoryQueryable = categoryQueryable.Include(c => c.RelatedPosts);

            if (user)
                categoryQueryable = categoryQueryable.Include(c => c.FollowerUsers);

            //Get the category object
            var category = categoryQueryable.FirstOrDefault();

            return Ok(category);
        }

        [HttpPost]
        public ActionResult<Category> Create([FromBody] CategoryInDto categoryInDto)
        {
            var categoryIn = _mapper.Map<Category>(categoryInDto);

            if (_categoryRepository.Add(categoryIn))
            {
                return categoryIn;
            }

            return BadRequest(error:"Error when creating category");
        }

        [HttpPost]
        public ActionResult<Category> AddUser([FromBody] UserCategoryDto userCategoryDto)
        {
            var userCategory = _mapper.Map<UserCategory>(userCategoryDto);

            if (_userCategoryRepository.Add(userCategory))
            {
                return Ok(userCategory);
            }

            return BadRequest(error: "Error when adding user-category relation");
        }

        [HttpPost("{id}")]
        public ActionResult<Category> Update(string id, [FromBody] CategoryInDto categoryInDto)
        {
            var categoryIn = _mapper.Map<Category>(categoryInDto);

            //Check if there exists a category with given id
            var category = _categoryRepository.GetById(Guid.Parse(id));
            if (category == null)
            {
                return NotFound("No such category with this id: " + id);
            }

            categoryIn.Id = Guid.Parse(id);

            //Update category
            if (_categoryRepository.Update(categoryIn))
            {
                return (categoryIn);
            }

            return BadRequest(error: "Error when updating category");
        }

        // POST api/users/delete/id
        [HttpPost("{id}")]
        public ActionResult<User> Delete(string id)
        {
            var categoryIn = _categoryRepository.GetById(Guid.Parse(id));

            //Check if there exists a category with given id
            if (categoryIn == null)
            {
                return NotFound("No such category with this id: " + id);
            }

            if (_categoryRepository.Delete(categoryIn))
            {
                return Ok("Category Deleted Successfully");
            }

            return BadRequest(error: "Error when deleting category");
        }

    }
}
