using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjectsLayer.Entity;
using DAOsLayer;
using NguyenKhanhMinhRazorPages.Services;

namespace NguyenKhanhMinhRazorPages.Pages.NewsArticlePages
{
    public class HistoryModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ISystemAccountService _systemAccountService;

        public HistoryModel(INewsArticleService newsArticleService, ISystemAccountService systemAccountService)
        {
            _newsArticleService = newsArticleService;
            _systemAccountService = systemAccountService;
        }

        public List<NewsArticle> NewsArticle { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToPage("/NotPermission");
            }
            var currentAccount = await _systemAccountService.GetAccountByEmail(userEmail);
            NewsArticle = await _newsArticleService.GetNewsArticlesByCreatedBy(currentAccount.AccountId);
            return Page();
        }
    }
}
