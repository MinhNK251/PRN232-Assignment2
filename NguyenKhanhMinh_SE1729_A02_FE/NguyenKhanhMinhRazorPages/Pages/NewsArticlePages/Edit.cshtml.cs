using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObjectsLayer.Entity;
using DAOsLayer;
using Microsoft.AspNetCore.SignalR;
using NguyenKhanhMinhRazorPages.Services;
using BusinessObjectsLayer.DTO;

namespace NguyenKhanhMinhRazorPages.Pages.NewsArticlePages
{
    public class EditModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ICategoryService _categoryService;
        private readonly ISystemAccountService _systemAccountService;
        private readonly ITagService _tagService;
        private readonly IHubContext<SignalrServer> _hubContext;

        public EditModel(INewsArticleService newsArticleService, ICategoryService categoryService, ISystemAccountService systemAccountService, ITagService tagService, IHubContext<SignalrServer> hubContext)
        {
            _newsArticleService = newsArticleService;
            _categoryService = categoryService;
            _systemAccountService = systemAccountService;
            _tagService = tagService;
            _hubContext = hubContext;
        }

        [BindProperty]
        public NewsArticle NewsArticle { get; set; } = default!;
        [BindProperty]
        public List<int> SelectedTags { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsArticle = await _newsArticleService.GetNewsArticleById(id);
            if (newsArticle == null)
            {
                return NotFound();
            }
            NewsArticle = newsArticle;
            SelectedTags = newsArticle.Tags.Select(t => t.TagId).ToList();
            ViewData["CategoryId"] = new SelectList(await _categoryService.GetCategories(), "CategoryId", "CategoryName");
            ViewData["Tags"] = new MultiSelectList(await _tagService.GetTags(), "TagId", "TagName");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["CategoryId"] = new SelectList(await _categoryService.GetCategories(), "CategoryId", "CategoryName");
                ViewData["Tags"] = new MultiSelectList(await _tagService.GetTags(), "TagId", "TagName");
                return Page();
            }

            try
            {
                // ✅ Avoid reloading NewsArticle
                var existingArticle = await _newsArticleService.GetNewsArticleById(NewsArticle.NewsArticleId);
                if (existingArticle == null)
                {
                    return NotFound();
                }
                var userEmail = HttpContext.Session.GetString("UserEmail");
                if (string.IsNullOrEmpty(userEmail))
                {
                    return RedirectToPage("/Login"); // Redirect to login if no session exists
                }
                var currentUser = await _systemAccountService.GetAccountByEmail(userEmail);
                var dto = new NewsArticleDto
                {
                    NewsArticleId = NewsArticle.NewsArticleId,
                    NewsTitle = NewsArticle.NewsTitle,
                    Headline = NewsArticle.Headline,
                    CreatedDate = existingArticle.CreatedDate, // preserve original
                    NewsContent = NewsArticle.NewsContent,
                    NewsSource = NewsArticle.NewsSource,
                    CategoryId = NewsArticle.CategoryId,
                    NewsStatus = NewsArticle.NewsStatus,
                    CreatedById = existingArticle.CreatedById, // preserve original
                    UpdatedById = currentUser.AccountId,
                    ModifiedDate = DateTime.Now,
                    TagIds = SelectedTags
                };

                await _newsArticleService.UpdateNewsArticle(dto);
                await _hubContext.Clients.All.SendAsync("LoadData");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _newsArticleService.GetNewsArticleById(NewsArticle.NewsArticleId) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToPage("./Index");
        }
    }
}
