using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjectsLayer.Entity;
using DAOsLayer;
using RepositoriesLayer;
using NguyenKhanhMinhRazorPages.Services;

namespace NguyenKhanhMinhRazorPages.Pages.SystemAccountPages
{
    public class DeleteModel : PageModel
    {
        private readonly ISystemAccountService _systemAccountService;
        private readonly INewsArticleService _newsArticleService;

        public DeleteModel(ISystemAccountService systemAccountService, INewsArticleService newsArticleService)
        {
            _systemAccountService = systemAccountService;
            _newsArticleService = newsArticleService;
        }

        [BindProperty]
        public SystemAccount SystemAccount { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(short id)
        {
            var systemaccount = await _systemAccountService.GetAccountById(id);
            if (systemaccount == null)
            {
                return NotFound();
            }
            SystemAccount = systemaccount;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(short id)
        {
            var systemaccount = await _systemAccountService.GetAccountById(id);
            if (systemaccount != null)
            {
                //SystemAccount = systemaccount;
                //foreach (var article in systemaccount.NewsArticles)
                //{
                //    await _newsArticleService.RemoveTagsByArticleId(article.NewsArticleId);
                //    await _newsArticleService.RemoveNewsArticle(article.NewsArticleId);
                //}
                if (systemaccount.NewsArticles.Any())
                {
                    SystemAccount = systemaccount;
                    TempData["ErrorMessage"] = "Cannot delete this account because it has created news articles.";
                    return Page();
                }
                await _systemAccountService.RemoveAccount(id);
            }

            return RedirectToPage("./Index");
        }
    }
}
