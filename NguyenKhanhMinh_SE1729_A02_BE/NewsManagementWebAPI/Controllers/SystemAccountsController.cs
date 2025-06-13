using Microsoft.AspNetCore.Mvc;
using ServiceLayer;
using BusinessObjectsLayer.Entity;
using Microsoft.Extensions.Options;
using NewsManagementWebAPI.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OData.Query;

namespace NewsManagementWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemAccountsController : ODataController
    {
        private readonly ISystemAccountService _context;
        private readonly IOptions<AdminAccountSettings> _adminAccountSettings;

        public SystemAccountsController(ISystemAccountService context, IOptions<AdminAccountSettings> adminAccountSettings)
        {
            _context = context;
            _adminAccountSettings = adminAccountSettings;
        }

        // GET: api/SystemAccounts
        [EnableQuery]
        [Authorize(Policy = "AdminOrStaffOrLecturer")]
        [HttpGet]
        public ActionResult<IEnumerable<SystemAccount>> GetSystemAccounts()
        {
            var accounts = _context.GetAccounts();
            return Ok(accounts);
        }

        // GET: api/SystemAccounts/5
        [Authorize(Policy = "AdminOrStaffOrLecturer")]
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
        [Authorize(Policy = "AdminOrStaffOrLecturer")]
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
        [Authorize(Policy = "AdminOrStaffOrLecturer")]
        [HttpPut]
        public IActionResult PutSystemAccount(SystemAccount systemAccount)
        {
            _context.UpdateAccount(systemAccount);
            return NoContent();
        }

        // POST: api/SystemAccounts
        [Authorize(Policy = "AdminOnly")]
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
        [Authorize(Policy = "AdminOnly")]
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
        public async Task<ActionResult> Login([FromBody] AccountRequestDto loginDTO)
        {
            // Post Login Request Body

            //{
            //    "email": "admin@CosmeticsDB.info",
            //    "password": "@1"
            //}

            try
            {
                var account = _context.Login(loginDTO.Email, loginDTO.Password, _adminAccountSettings);
                if (account == null)
                {
                    return Unauthorized("Invalid email or password.");
                }

                IConfiguration configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true).Build();

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, account.AccountEmail),
                    new Claim("Role", account.AccountRole.ToString()),
                    new Claim("AccountId", account.AccountId.ToString()),
                };

                var symetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
                var signCredential = new SigningCredentials(symetricKey, SecurityAlgorithms.HmacSha256);

                var preparedToken = new JwtSecurityToken(
                    issuer: configuration["JWT:Issuer"],
                    audience: configuration["JWT:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(16),
                    signingCredentials: signCredential);

                var generatedToken = new JwtSecurityTokenHandler().WriteToken(preparedToken);
                var role = account.AccountRole;
                var accountId = account.AccountId.ToString();

                return Ok(new AccountResponseDto
                {
                    UserEmail = account.AccountEmail,
                    UserName = account.AccountName,
                    Role = role,
                    Token = generatedToken,
                    AccountId = accountId
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
