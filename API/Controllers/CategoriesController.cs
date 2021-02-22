using System;
using API.Models;
using API.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace API.Controllers
{
    [Route("category")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService _categoryService = new CategoryService();

        [HttpGet]
        public async Task<List<Category>> GetCategories() => await _categoryService.GetCategories();

        [HttpPost("new")]
        public async Task<ObjectResult> CreateCategory([FromBody] Category category)
        {
            try
            {
                if (string.IsNullOrEmpty(category.Name.Trim()))
                    return BadRequest("Category cannot be empty");

                await _categoryService.InsertCategory(category.Name);

                return Created("category/new", category);
            }
            catch (Exception)
            {
                return StatusCode(500, "Unexpected error");
            }
        }
    }
}
