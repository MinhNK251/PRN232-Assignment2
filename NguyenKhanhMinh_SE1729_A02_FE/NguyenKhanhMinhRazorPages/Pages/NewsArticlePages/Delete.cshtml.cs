using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using NguyenKhanhMinhRazorPages.Entity;
using NguyenKhanhMinhRazorPages.Services;

namespace NguyenKhanhMinhRazorPages.Pages.NewsArticlePages
{
    public class DeleteModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ITagService _tagService;
        private readonly IHubContext<SignalrServer> _hubContext;

        public DeleteModel(INewsArticleService newsArticleService, ITagService tagService, IHubContext<SignalrServer> hubContext)
        {
            _newsArticleService = newsArticleService;
            _tagService = tagService;
            _hubContext = hubContext;
        }

        public NewsArticle NewsArticle { get; set; } = default!;
        public List<Tag> Tags { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var newsArticle = await _newsArticleService.GetNewsArticleById(id);
            if (newsArticle == null)
            {
                return NotFound();
            }

            NewsArticle = newsArticle;

            // Fetch related tags
            Tags = await _tagService.GetTagsByNewsArticleId(id);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var existingArticle = await _newsArticleService.GetNewsArticleById(id);
            if (existingArticle != null)
            {
                await _newsArticleService.RemoveNewsArticle(id);
                await _hubContext.Clients.All.SendAsync("LoadData");
            }

            return RedirectToPage("./Index");
        }
    }
}
