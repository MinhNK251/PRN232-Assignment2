using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjectsLayer.Entity;
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
