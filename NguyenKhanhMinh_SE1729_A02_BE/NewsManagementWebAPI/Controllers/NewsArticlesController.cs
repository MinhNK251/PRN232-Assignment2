using Microsoft.AspNetCore.Mvc;
using BusinessObjectsLayer.Entity;
using ServiceLayer;
using Microsoft.Extensions.Logging;
using BusinessObjectsLayer.DTO;

namespace NewsManagementWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsArticlesController : ControllerBase
    {
        private readonly INewsArticleService _service;
        private readonly ITagService _tagService;
        private readonly ILogger<NewsArticlesController> _logger;

        public NewsArticlesController(INewsArticleService service, ITagService tagService, ILogger<NewsArticlesController> logger)
        {
            _service = service;
            _tagService = tagService;
            _logger = logger;
        }

        // GET: api/NewsArticles
        [HttpGet]
        public ActionResult<IEnumerable<NewsArticle>> GetNewsArticles()
        {
            _logger.LogInformation("GET request received for all news articles");
            var articles = _service.GetNewsArticles();
            return Ok(articles);
        }

        // GET: api/NewsArticles/CreatedBy
        [HttpGet("created-by/{userId}")]
        public ActionResult<IEnumerable<NewsArticle>> GetNewsArticlesCreatedBy(int userId)
        {
            _logger.LogInformation("GET request received for all news articles created by login user");
            var articles = _service.GetNewsArticlesByCreatedBy(userId);
            return Ok(articles);
        }

        // GET: api/NewsArticles/active
        [HttpGet("active")]
        public ActionResult<IEnumerable<NewsArticle>> GetActiveNewsArticles()
        {
            _logger.LogInformation("GET request received for active news articles");
            var articles = _service.GetActiveNewsArticles();
            return Ok(articles);
        }

        // GET: api/NewsArticles/by-tag/5
        [HttpGet("by-tag/{tagId}")]
        public ActionResult<IEnumerable<NewsArticle>> GetArticlesByTagId(int tagId)
        {
            _logger.LogInformation($"GET request received for news articles with tag ID: {tagId}");
            var articles = _service.GetArticlesByTagId(tagId);
            return Ok(articles);
        }

        // GET: api/NewsArticles/by-date-range
        [HttpGet("by-date-range")]
        public ActionResult<IEnumerable<NewsArticle>> GetNewsArticlesByDateRange([FromQuery] DateRangeDto range)
        {
            _logger.LogInformation($"GET request received for news articles from {range.StartDate} to {range.EndDate}");

            if (range.StartDate > range.EndDate)
            {
                _logger.LogWarning("Invalid date range");
                return BadRequest("Start date cannot be after end date.");
            }

            var articles = _service.GetNewsArticlesByDateRange(range.StartDate, range.EndDate);
            return Ok(articles);
        }

        // GET: api/NewsArticles/5
        [HttpGet("{id}")]
        public ActionResult<NewsArticle> GetNewsArticle(string id)
        {
            _logger.LogInformation($"GET request received for news article with ID: {id}");
            var article = _service.GetNewsArticleById(id);

            if (article == null)
            {
                _logger.LogWarning($"News article with ID: {id} not found");
                return NotFound();
            }

            return Ok(article);
        }

        // PUT: api/NewsArticles/5
        [HttpPut("{id}")]
        public ActionResult<NewsArticle> PutNewsArticle(String id, [FromBody] NewsArticleDto dto)
        {
            var article = new NewsArticle
            {
                NewsArticleId = dto.NewsArticleId,
                NewsTitle = dto.NewsTitle,
                Headline = dto.Headline,
                CreatedDate = dto.CreatedDate,
                NewsContent = dto.NewsContent,
                NewsSource = dto.NewsSource,
                CategoryId = dto.CategoryId,
                NewsStatus = dto.NewsStatus,
                CreatedById = dto.CreatedById,
                UpdatedById = dto.UpdatedById,
                ModifiedDate = dto.ModifiedDate,
                Tags = _tagService.GetTagsByIds(dto.TagIds ?? new List<int>())
            };
            _service.RemoveTagsByArticleId(id);
            _service.UpdateNewsArticle(id, article);
            return NoContent();
        }

        // POST: api/NewsArticles
        [HttpPost]
        public ActionResult<NewsArticle> PostNewsArticle([FromBody] NewsArticleDto dto)
        {
            var article = new NewsArticle
            {
                NewsArticleId = dto.NewsArticleId,
                NewsTitle = dto.NewsTitle,
                Headline = dto.Headline,
                CreatedDate = dto.CreatedDate,
                NewsContent = dto.NewsContent,
                NewsSource = dto.NewsSource,
                CategoryId = dto.CategoryId,
                NewsStatus = dto.NewsStatus,
                CreatedById = dto.CreatedById,
                UpdatedById = dto.UpdatedById,
                ModifiedDate = dto.ModifiedDate,
                Tags = _tagService.GetTagsByIds(dto.TagIds ?? new List<int>())
            };

            _service.AddNewsArticle(article);

            return CreatedAtAction(nameof(GetNewsArticle), new { id = article.NewsArticleId }, article);
        }

        // DELETE: api/NewsArticles/5
        [HttpDelete("{id}")]
        public IActionResult DeleteNewsArticle(string id)
        {
            var existing = _service.GetNewsArticleById(id);
            if (existing == null)
            {
                return NotFound();
            }

            try
            {
                _service.RemoveTagsByArticleId(id);
                _service.RemoveNewsArticle(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting article ID: {id}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/NewsArticles/{id}/tags
        [HttpDelete("{id}/tags")]
        public IActionResult RemoveTagsByArticleId(string id)
        {
            _logger.LogInformation($"DELETE request received to remove tags from article ID: {id}");
            var article = _service.GetNewsArticleById(id);

            if (article == null)
            {
                _logger.LogWarning($"Article with ID: {id} not found");
                return NotFound($"Article with ID: {id} does not exist");
            }

            _service.RemoveTagsByArticleId(id);
            _logger.LogInformation($"Tags removed from article ID: {id}");

            return NoContent();
        }
    }
}
