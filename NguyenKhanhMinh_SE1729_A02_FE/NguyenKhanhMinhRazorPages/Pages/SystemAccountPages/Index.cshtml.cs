using Microsoft.AspNetCore.Mvc.RazorPages;
using NguyenKhanhMinhRazorPages.Entity;
using NguyenKhanhMinhRazorPages.Services;

namespace NguyenKhanhMinhRazorPages.Pages.SystemAccountPages
{
    public class IndexModel : PageModel
    {
        private readonly ISystemAccountService _systemAccountService;

        public IndexModel(ISystemAccountService systemAccountService)
        {
            _systemAccountService = systemAccountService;
        }

        public IList<SystemAccount> SystemAccount { get; set; } = default!;

        public async Task OnGetAsync()
        {
            SystemAccount = await _systemAccountService.GetAccounts();
        }
    }
}
