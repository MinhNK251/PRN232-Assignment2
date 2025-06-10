using Microsoft.AspNetCore.Mvc;
using ServiceLayer;
using BusinessObjectsLayer.Entity;
using Microsoft.Extensions.Options;
using BusinessObjectsLayer.DTO;

namespace NewsManagementWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemAccountsController : ControllerBase
    {
        private readonly ISystemAccountService _context;
        private readonly IOptions<AdminAccountSettings> _adminAccountSettings;

        public SystemAccountsController(ISystemAccountService context, IOptions<AdminAccountSettings> adminAccountSettings)
        {
            _context = context;
            _adminAccountSettings = adminAccountSettings;
        }

        // GET: api/SystemAccounts
        [HttpGet]
        public ActionResult<IEnumerable<SystemAccount>> GetSystemAccounts()
        {
            var accounts = _context.GetAccounts();
            return Ok(accounts);
        }

        // GET: api/SystemAccounts/5
        [HttpGet("{id}")]
        public ActionResult<SystemAccount> GetSystemAccount(short id)
        {
            var account = _context.GetAccountById(id);

            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }

        // GET: api/SystemAccounts/email
        [HttpGet("email")]
        public ActionResult<SystemAccount> GetSystemAccountByEmail([FromQuery] String email)
        {
            var account = _context.GetAccountByEmail(email);

            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }

        // PUT: api/SystemAccounts
        [HttpPut]
        public IActionResult PutSystemAccount(SystemAccount systemAccount)
        {
            _context.UpdateAccount(systemAccount);
            return NoContent();
        }

        // POST: api/SystemAccounts
        [HttpPost]
        public ActionResult<SystemAccount> PostSystemAccount(SystemAccount systemAccount)
        {
            if (_context.GetAccountById(systemAccount.AccountId) != null)
            {
                return Conflict("An account with this ID already exists.");
            }

            _context.AddAccount(systemAccount);
            return CreatedAtAction(nameof(GetSystemAccount), new { id = systemAccount.AccountId }, systemAccount);
        }

        // DELETE: api/SystemAccounts/5
        [HttpDelete("{id}")]
        public IActionResult DeleteSystemAccount(short id)
        {
            var account = _context.GetAccountById(id);
            if (account == null)
            {
                return NotFound();
            }

            _context.RemoveAccount(id);
            return NoContent();
        }

        // POST: api/SystemAccounts
        [HttpPost("login")]
        public ActionResult<SystemAccount?> Login([FromBody] LoginDto dto)
        {
            var account = _context.Login(dto.Email, dto.Password, _adminAccountSettings);

            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }
    }
}
