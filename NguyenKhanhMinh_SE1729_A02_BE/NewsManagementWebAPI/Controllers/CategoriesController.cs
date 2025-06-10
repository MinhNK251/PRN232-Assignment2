using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BusinessObjectsLayer.Entity;
using ServiceLayer;

namespace NewsManagementWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoriesController(ICategoryService service)
        {
            _service = service;
        }

        // GET: api/Categories
        [HttpGet]
        public ActionResult<IEnumerable<Category>> GetCategories()
        {
            var categories = _service.GetCategories();
            return Ok(categories);
        }

        // GET: api/Categories/active
        [HttpGet("active")]
        public ActionResult<IEnumerable<Category>> GetActiveCategories()
        {
            var categories = _service.GetActiveCategories();
            return Ok(categories);
        }


        // GET: api/Categories/5
        [HttpGet("{id}")]
        public ActionResult<Category> GetCategory(short id)
        {
            var category = _service.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        // PUT: api/Categories/5
        [HttpPut]
        public IActionResult PutCategory(Category category)
        {
            _service.UpdateCategory(category);
            return NoContent();
        }

        // POST: api/Categories
        [HttpPost]
        public ActionResult<Category> PostCategory(Category category)
        {
            _service.AddCategory(category);
            return CreatedAtAction(nameof(GetCategory), new { id = category.CategoryId }, category);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(short id)
        {
            var existing = _service.GetCategoryById(id);
            if (existing == null)
            {
                return NotFound();
            }

            _service.RemoveCategory(id);
            return NoContent();
        }
    }
}
