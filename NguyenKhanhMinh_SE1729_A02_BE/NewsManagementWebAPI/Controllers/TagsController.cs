using Azure;
using BusinessObjectsLayer.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer;

namespace NewsManagementWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _service;

        public TagsController(ITagService service)
        {
            _service = service;
        }

        // GET: api/Tags
        [HttpGet]
        public ActionResult<IEnumerable<Tag>> GetTags()
        {
            var categories = _service.GetTags();
            return Ok(categories);
        }

        // GET: api/Tags/by-ids
        [HttpGet("by-ids")]
        public ActionResult<List<Tag>> GetTagsByIds([FromBody] List<int> tagIds)
        {
            var tags = _service.GetTagsByIds(tagIds);
            return Ok(tags);
        }

        // GET: api/Tags/news/{newsId}
        [HttpGet("article/{articleId}")]
        public ActionResult<List<Tag>> GetTagsByNewsArticleId(string articleId)
        {
            var tags = _service.GetTagsByNewsArticleId(articleId);
            return Ok(tags);
        }

        // GET: api/Tags/5
        [HttpGet("{id}")]
        public ActionResult<Tag> GetTagById(int id)
        {
            var tag = _service.GetTagById(id);
            if (tag == null)
            {
                return NotFound();
            }
            return Ok(tag);
        }

        // PUT: api/Tags/5
        [HttpPut]
        public IActionResult PutTag(Tag tag)
        {
            try
            {
                _service.UpdateTag(tag);
            }
            catch
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Tags
        [HttpPost]
        public ActionResult<Tag> PostTag(Tag tag)
        {
            _service.AddTag(tag);
            return CreatedAtAction(nameof(GetTagById), new { id = tag.TagId }, tag);
        }

        // DELETE: api/Tags/5
        [HttpDelete("{id}")]
        public IActionResult DeleteTag(int id)
        {
            var existing = _service.GetTagById(id);
            if (existing == null)
            {
                return NotFound();
            }
            _service.RemoveArticlesByTagId(id);
            _service.RemoveTag(id);
            return NoContent();
        }

        // DELETE: api/Tags/{id}/articles
        [HttpDelete("{id}/articles")]
        public IActionResult RemoveArticlesByTagId(int id)
        {;
            _service.RemoveArticlesByTagId(id);
            return NoContent();
        }
    }
}
