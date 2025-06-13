using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BusinessObjectsLayer.Entity;
using ServiceLayer;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OData.Query;

namespace NewsManagementWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ODataController
    {
        private readonly ICategoryService _service;

        public CategoriesController(ICategoryService service)
        {
            _service = service;
        }

        // GET: api/Categories
        [EnableQuery]
        [Authorize(Policy = "AdminOrStaffOrLecturer")]
        [HttpGet]
        public ActionResult<IEnumerable<Category>> GetCategories()
        {
            var categories = _service.GetCategories();
            return Ok(categories);
        }

        // GET: api/Categories/active
        [Authorize(Policy = "AdminOrStaffOrLecturer")]
        [HttpGet("active")]
        public ActionResult<IEnumerable<Category>> GetActiveCategories()
        {
            var categories = _service.GetActiveCategories();
            return Ok(categories);
        }


        // GET: api/Categories/5
        [Authorize(Policy = "AdminOrStaffOrLecturer")]
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
        [Authorize(Policy = "StaffOnly")]
        [HttpPut]
        public IActionResult PutCategory(Category category)
        {
            _service.UpdateCategory(category);
            return NoContent();
        }

        // POST: api/Categories
        [Authorize(Policy = "StaffOnly")]
        [HttpPost]
        public ActionResult<Category> PostCategory(Category category)
        {
            _service.AddCategory(category);
            return CreatedAtAction(nameof(GetCategory), new { id = category.CategoryId }, category);
        }

        // DELETE: api/Categories/5
        [Authorize(Policy = "StaffOnly")]
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
