using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjectsLayer.Entity;
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
