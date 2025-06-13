using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NguyenKhanhMinhRazorPages.Entity;
using NguyenKhanhMinhRazorPages.Services;

namespace NguyenKhanhMinhRazorPages.Pages.SystemAccountPages
{
    public class DetailsModel : PageModel
    {
        private readonly ISystemAccountService _systemAccountService;

        public DetailsModel(ISystemAccountService systemAccountService)
        {
            _systemAccountService = systemAccountService;
        }

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
    }
}
