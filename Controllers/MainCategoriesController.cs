using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blog_Project.Dtos.CategoryDtos;
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
    public class MainCategoriesController : ControllerBase
    {
        private readonly IRepository<MainCategory> _mainCategoryRepository;
        private readonly IMapper _mapper;

        public MainCategoriesController(IRepository<MainCategory> mainCategoryRepository, IMapper mapper)
        {
            _mainCategoryRepository = mainCategoryRepository;
            _mapper = mapper;
        }

        /**
         * Gives response including all main categories with their subcategories
         */
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_mainCategoryRepository.All().Include(mc => mc.SubCategories)
                .Select(mc => _mapper.Map<MainCategoryOutDto>(mc)));
        }

        [HttpPost]
        public ActionResult<MainCategoryOutDto> Create([FromBody] MainCategoryCreateDto mainCategoryIn)
        {

            //Check if request is sent by admin.
            if (!AuthorizationHelpers.IsAdmin(HttpContext.User))
            {
                return Unauthorized(new Message("Unauthorized user."));
            }

            //Check if request is valid.
            if (string.IsNullOrEmpty(mainCategoryIn.Name))
            {
                return BadRequest(new Message("Please give a valid name."));
            }

            if (_mainCategoryRepository.Where(mc => mc.Name == mainCategoryIn.Name).Any())
            {
                return BadRequest(new Message("There is already a main category with name: " + mainCategoryIn.Name));
            }

            var mainCategory = _mapper.Map<MainCategory>(mainCategoryIn);

            if (_mainCategoryRepository.Add(mainCategory))
            {
                var mainCategoryOutDto = _mapper.Map<MainCategoryOutDto>(mainCategory);

                return Ok(mainCategoryOutDto);
            }

            return BadRequest(new Message("Error when creating main category"));
        }

        [HttpPost("{id}")]
        public ActionResult<MainCategoryOutDto> Update(string id, [FromBody] MainCategoryCreateDto mainCategoryIn)
        {
            //Check if request is sent by admin.
            if (!AuthorizationHelpers.IsAdmin(HttpContext.User))
            {
                return Unauthorized(new Message("Unauthorized user."));
            }

            var mainCategory = _mainCategoryRepository.GetById(id);

            if (mainCategory == null)
            {
                return NotFound(new Message("There is no main category with id: " + id));
            }

            if (!string.IsNullOrEmpty(mainCategoryIn.Name))
            {
                mainCategory.Name = mainCategoryIn.Name;
            }

            if (_mainCategoryRepository.Update(mainCategory))
            {
                var mainCategoryOutDto = _mapper.Map<MainCategoryOutDto>(mainCategory);

                return Ok(mainCategoryOutDto);
            }

            return BadRequest(new Message("Error when updating main category with id: " + id));
        }

        [HttpPost("{id}")]
        public IActionResult Delete(string id)
        {
            //Check if request is sent by admin.
            if (!AuthorizationHelpers.IsAdmin(HttpContext.User))
            {
                return Unauthorized(new Message("Unauthorized user."));
            }

            var mainCategory = _mainCategoryRepository.GetById(id);

            if (mainCategory == null)
            {
                return NotFound(new Message("There is no main category with id: " + id));
            }

            if (_mainCategoryRepository.Delete(mainCategory))
            {
                return Ok(new Message("Main category with name: " + mainCategory.Name + " and with id: " + mainCategory.Id +
                                      " is deleted successfully"));
            }

            return BadRequest(new Message("Error when updating main category with id: " + id));
        }
    }
}
