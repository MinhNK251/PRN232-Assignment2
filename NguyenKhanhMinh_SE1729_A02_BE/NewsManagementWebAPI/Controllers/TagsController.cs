﻿using Azure;
using BusinessObjectsLayer.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using ServiceLayer;

namespace NewsManagementWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ODataController
    {
        private readonly ITagService _service;

        public TagsController(ITagService service)
        {
            _service = service;
        }

        // GET: api/Tags
        [EnableQuery]
        [Authorize(Policy = "AdminOrStaffOrLecturer")]
        [HttpGet]
        public ActionResult<IEnumerable<Tag>> GetTags()
        {
            var categories = _service.GetTags();
            return Ok(categories);
        }

        // GET: api/Tags/by-ids
        [Authorize(Policy = "AdminOrStaffOrLecturer")]
        [HttpGet("by-ids")]
        public ActionResult<List<Tag>> GetTagsByIds([FromBody] List<int> tagIds)
        {
            var tags = _service.GetTagsByIds(tagIds);
            return Ok(tags);
        }

        // GET: api/Tags/news/{newsId}
        [Authorize(Policy = "AdminOrStaffOrLecturer")]
        [HttpGet("article/{articleId}")]
        public ActionResult<List<Tag>> GetTagsByNewsArticleId(string articleId)
        {
            var tags = _service.GetTagsByNewsArticleId(articleId);
            return Ok(tags);
        }

        // GET: api/Tags/5
        [Authorize(Policy = "AdminOrStaffOrLecturer")]
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
        [Authorize(Policy = "StaffOnly")]
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
        [Authorize(Policy = "StaffOnly")]
        [HttpPost]
        public ActionResult<Tag> PostTag(Tag tag)
        {
            _service.AddTag(tag);
            return CreatedAtAction(nameof(GetTagById), new { id = tag.TagId }, tag);
        }

        // DELETE: api/Tags/5
        [Authorize(Policy = "StaffOnly")]
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
        [Authorize(Policy = "StaffOnly")]
        [HttpDelete("{id}/articles")]
        public IActionResult RemoveArticlesByTagId(int id)
        {;
            _service.RemoveArticlesByTagId(id);
            return NoContent();
        }
    }
}
