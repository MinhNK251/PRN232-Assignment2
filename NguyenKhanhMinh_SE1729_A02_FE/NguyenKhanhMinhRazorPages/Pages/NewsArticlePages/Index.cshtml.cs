using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text.Json.Serialization;
using NguyenKhanhMinhRazorPages.Services;
using NguyenKhanhMinhRazorPages.Entity;

namespace NguyenKhanhMinhRazorPages.Pages.NewsArticlePages
{
    public class IndexModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ISystemAccountService _systemAccountService;

        public IndexModel(INewsArticleService newsArticleService, ISystemAccountService systemAccountService)
        {
            _newsArticleService = newsArticleService;
            _systemAccountService = systemAccountService;
        }

        public IList<NewsArticle> NewsArticle { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public string? SearchTitle { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            var articles = string.IsNullOrEmpty(userRole) || userRole.Equals("2")
                ? await _newsArticleService.GetActiveNewsArticles()
                : await _newsArticleService.GetNewsArticles();
            NewsArticle = articles;
            return Page();
        }

        public async Task<JsonResult> OnGetLoadDataAsync(string? searchTitle)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            var articles = string.IsNullOrEmpty(userRole) || userRole.Equals("2")
                ? await _newsArticleService.GetActiveNewsArticles()
                : await _newsArticleService.GetNewsArticles();
            if (!string.IsNullOrEmpty(searchTitle))
            {
                articles = articles.Where(a => a.NewsTitle != null &&
                                a.NewsTitle.Contains(searchTitle, StringComparison.CurrentCultureIgnoreCase))
                    .ToList();
            }
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve, // Handle circular references
                WriteIndented = true // Optional: Makes the JSON output more readable
            };

            return new JsonResult(new { articles }, options);
        }
    }
}
