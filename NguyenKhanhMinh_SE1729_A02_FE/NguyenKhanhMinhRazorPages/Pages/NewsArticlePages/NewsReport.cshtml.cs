using BusinessObjectsLayer.DTO;
using BusinessObjectsLayer.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NguyenKhanhMinhRazorPages.Services;
using System;
using System.Collections.Generic;

namespace NguyenKhanhMinhRazorPages.Pages.NewsArticlePages
{
    public class NewsReportModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;

        public NewsReportModel(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

        [BindProperty]
        public DateTime StartDate { get; set; } = new DateTime(2024, 1, 1);

        [BindProperty]
        public DateTime EndDate { get; set; } = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);

        public List<NewsArticle> NewsArticles { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            DateRangeDto requestBody = new DateRangeDto
            {
                StartDate = this.StartDate,
                EndDate = this.EndDate
            };
            NewsArticles = await _newsArticleService.GetNewsArticlesByDateRange(requestBody);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            DateRangeDto requestBody = new DateRangeDto
            {
                StartDate = this.StartDate,
                EndDate = this.EndDate
            };
            NewsArticles = await _newsArticleService.GetNewsArticlesByDateRange(requestBody);
            return Page();
        }
    }
}
