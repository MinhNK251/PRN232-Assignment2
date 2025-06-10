using BusinessObjectsLayer.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using NguyenKhanhMinhRazorPages.Services;
using RepositoriesLayer;
using System.Text.Json;
using System.Text;
using BusinessObjectsLayer.DTO;

namespace NguyenKhanhMinhRazorPages.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ISystemAccountService _accountService;

        public LoginModel(ISystemAccountService accountService)
        {
            _accountService = accountService;
        }

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public string? ErrorMessage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Email and password are required.";
                return Page();
            }

            LoginDto requestBody = new LoginDto
            {
                Email = this.Email,
                Password = this.Password
            };
            var response = await _accountService.Login(requestBody);
            if (response != null)
            {
                HttpContext.Session.SetString("UserEmail", response.AccountEmail);
                HttpContext.Session.SetString("UserRole", response.AccountRole.ToString());
                HttpContext.Session.SetString("UserName", response.AccountName);
                HttpContext.Session.SetString("AccountId", response.AccountId.ToString());

                // Redirect by role
                return response.AccountRole == 0
                    ? RedirectToPage("/SystemAccountPages/Index")
                    : RedirectToPage("/NewsArticlePages/Index");
            }
            else
            {
                ErrorMessage = "Invalid email or password.";
                return Page();
            }
        }
    }
}
