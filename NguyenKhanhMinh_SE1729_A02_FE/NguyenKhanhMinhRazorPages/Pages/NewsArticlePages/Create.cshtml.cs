using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObjectsLayer.Entity;
using Microsoft.AspNetCore.SignalR;
using NguyenKhanhMinhRazorPages.Services;
using BusinessObjectsLayer.DTO;
using DAOsLayer;

namespace NguyenKhanhMinhRazorPages.Pages.NewsArticlePages
{
    public class CreateModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ICategoryService _categoryService;
        private readonly ISystemAccountService _systemAccountService;
        private readonly ITagService _tagService;
        private readonly IHubContext<SignalrServer> _hubContext;

        public CreateModel(INewsArticleService newsArticleService, ICategoryService categoryService, ISystemAccountService systemAccountService, ITagService tagService, IHubContext<SignalrServer> hubContext)
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
        public List<int>? SelectedTags { get; set; } = [];

        public async Task<IActionResult> OnGetAsync()
        {
            await PopulateViewDataAsync(); 
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await PopulateViewDataAsync(); 
                return Page();
            }
            var existingArticle = await _newsArticleService.GetNewsArticleById(NewsArticle.NewsArticleId);
            if (existingArticle != null)
            {
                ModelState.AddModelError("NewsArticle.NewsArticleId", "This NewsArticle ID already exists. Please enter a unique ID.");
                await PopulateViewDataAsync(); 
                return Page();
            }

            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToPage("/Login");
            }

            var account = await _systemAccountService.GetAccountByEmail(userEmail);
            var dto = new NewsArticleDto
            {
                NewsArticleId = NewsArticle.NewsArticleId,
                NewsTitle = NewsArticle.NewsTitle,
                Headline = NewsArticle.Headline,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                NewsContent = NewsArticle.NewsContent,
                NewsSource = NewsArticle.NewsSource,
                CategoryId = NewsArticle.CategoryId,
                NewsStatus = NewsArticle.NewsStatus,
                CreatedById = account.AccountId,
                UpdatedById = account.AccountId,
                TagIds = SelectedTags ?? new List<int>()
            };
            await _newsArticleService.AddNewsArticle(dto);
            await _hubContext.Clients.All.SendAsync("LoadData");
            return RedirectToPage("./Index");
        }

        private async Task PopulateViewDataAsync()
        {
            ViewData["CategoryId"] = new SelectList(await _categoryService.GetCategories(), "CategoryId", "CategoryName");
            ViewData["Tags"] = new MultiSelectList(await _tagService.GetTags(), "TagId", "TagName");
        }
    }
}
