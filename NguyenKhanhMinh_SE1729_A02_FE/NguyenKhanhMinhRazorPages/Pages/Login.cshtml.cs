using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NguyenKhanhMinhRazorPages.Services;
using NguyenKhanhMinhRazorPages.DTOs;

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

            AccountRequestDto requestBody = new AccountRequestDto
            {
                Email = this.Email,
                Password = this.Password
            };
            var response = await _accountService.Login(requestBody);
            if (response != null)
            {
                HttpContext.Session.SetString("JWToken", response.Token);
                HttpContext.Session.SetString("UserEmail", response.UserEmail);
                HttpContext.Session.SetString("UserName", response.UserName);
                HttpContext.Session.SetString("UserRole", response.Role.ToString());
                HttpContext.Session.SetString("AccountId", response.AccountId.ToString());

                // Redirect by role
                return response.Role == 0
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
