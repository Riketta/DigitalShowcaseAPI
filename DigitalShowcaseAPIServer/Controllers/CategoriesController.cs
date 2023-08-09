using DigitalShowcaseAPIServer.Data.Interfaces;
using DigitalShowcaseAPIServer.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace DigitalShowcaseAPIServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _api;

        public CategoriesController(ICategoryService api)
        {
            _api = api;
        }

        [HttpGet(Name = "GetAllCategories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Category>>> GetAllCategories() // IActionResult
        {
            return Ok(await _api.GetCategoriesAsync());
        }

        [HttpGet("{id}", Name = "GetCategoryById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCategoryById(Category.CategoryId id)
        {
            Category? category = await _api.GetCategoryAsync(id);
            if (category is null)
                return NotFound($"No category with such id: {id}");
            
            return Ok(category);
        }
    }
}
