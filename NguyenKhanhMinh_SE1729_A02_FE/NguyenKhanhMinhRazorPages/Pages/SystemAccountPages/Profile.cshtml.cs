using BusinessObjectsLayer.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NguyenKhanhMinhRazorPages.Services;

namespace NguyenKhanhMinhRazorPages.Pages.SystemAccountPages
{
    public class ProfileModel : PageModel
    {
        private readonly ISystemAccountService _systemAccountService;

        public ProfileModel(ISystemAccountService systemAccountService)
        {
            _systemAccountService = systemAccountService;
        }

        [BindProperty]
        public SystemAccount SystemAccount { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string? returnUrl = "/NewsArticlePages/Index") // Default Page
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToPage("/NotPermission");
            }

            var currentUser = await _systemAccountService.GetAccountByEmail(userEmail);
            SystemAccount = currentUser;
            ViewData["ReturnUrl"] = returnUrl; // Store returnUrl in ViewData
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = "/NewsArticlePages/Index")
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                HttpContext.Session.SetString("UserName", SystemAccount.AccountName.ToString());
                await _systemAccountService.UpdateAccount(SystemAccount);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _systemAccountService.GetAccountById(SystemAccount.AccountId) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage(returnUrl);
        }
    }
}
