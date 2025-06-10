using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObjectsLayer.Entity;
using DAOsLayer;
using RepositoriesLayer;
using NguyenKhanhMinhRazorPages.Services;

namespace NguyenKhanhMinhRazorPages.Pages.SystemAccountPages
{
    public class CreateModel : PageModel
    {
        private readonly ISystemAccountService _systemAccountService;

        public CreateModel(ISystemAccountService systemAccountService)
        {
            _systemAccountService = systemAccountService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public SystemAccount SystemAccount { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (!new EmailAddressAttribute().IsValid(SystemAccount.AccountEmail))
            {
                ModelState.AddModelError("SystemAccount.AccountEmail", "Invalid email format.");
                return Page();
            }
            if (await _systemAccountService.GetAccountById(SystemAccount.AccountId) != null)
            {
                ModelState.AddModelError("SystemAccount.AccountId", "This Account ID already exists. Please enter a unique ID.");
                return Page();
            }
            if (await _systemAccountService.GetAccountByEmail(SystemAccount.AccountEmail) != null)
            {
                ModelState.AddModelError("SystemAccount.AccountEmail", "This Email already exists. Please enter another Email.");
                return Page();
            }
            await _systemAccountService.AddAccount(SystemAccount);

            return RedirectToPage("./Index");
        }
    }
}
