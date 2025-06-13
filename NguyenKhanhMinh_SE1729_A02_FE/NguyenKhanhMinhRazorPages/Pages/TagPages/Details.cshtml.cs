using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NguyenKhanhMinhRazorPages.Entity;
using NguyenKhanhMinhRazorPages.Services;

namespace NguyenKhanhMinhRazorPages.Pages.TagPages
{
    public class DetailsModel : PageModel
    {
        private readonly ITagService _tagService;
        private readonly INewsArticleService _newsArticleService;

        public DetailsModel(ITagService tagService, INewsArticleService newsArticleService)
        {
            _tagService = tagService;
            _newsArticleService = newsArticleService;
        }

        public Tag Tag { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var tag = await _tagService.GetTagById(id);
            if (tag == null)
            {
                return NotFound();
            }
            Tag = tag;
            return Page();
        }
    }
}
