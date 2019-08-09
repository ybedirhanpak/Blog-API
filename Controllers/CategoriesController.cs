using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog_Project.Models;
using Blog_Project.Services;
using Microsoft.AspNetCore.Mvc;

namespace Blog_Project.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IRepository<Category> _categoryRepository;

        public CategoriesController(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public ActionResult<List<Category>> GetAll()
        {
            return _categoryRepository.All().OrderBy(c => c.Name).ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Category> Get(string id)
        {
            var category = _categoryRepository.GetById(Guid.Parse(id));
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }
        
        [HttpPost]
        public ActionResult<Category> Create([FromBody] Category categoryIn)
        {
            if (_categoryRepository.Add(categoryIn))
            {
                return categoryIn;
            }

            return BadRequest();
        }

        [HttpPost("{id}")]
        public ActionResult<Category> Update(string id, [FromBody] Category categoryIn)
        {
            var categoryOld = _categoryRepository.GetById(Guid.Parse(id));
            if (categoryOld == null)
            {
                return NotFound();
            }

            categoryIn.Id = Guid.Parse(id);

            if (_categoryRepository.Update(categoryIn))
            {
                return (categoryIn);
            }
            return BadRequest();
        }

        // POST api/users/delete/id
        [HttpPost("{id}")]
        public ActionResult<User> Delete(string id)
        {
            var categoryIn = _categoryRepository.GetById(Guid.Parse(id));

            if (categoryIn != null && _categoryRepository.Delete(categoryIn))
            {
                return Ok();
            }
            return BadRequest();
        }

    }
}
