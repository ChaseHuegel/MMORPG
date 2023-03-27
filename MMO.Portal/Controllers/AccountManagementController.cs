using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MMO.Bridge.Types;
using MMO.Bridge.Util;
using MMO.Portal.Models;

namespace MMO.Portal.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountManagementController : ControllerBase
    {
        private readonly PortalDbContext _context;
        private readonly UserManager _userManager;

        public AccountManagementController(PortalDbContext context, UserManager userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("Info")]
        public async Task<ActionResult<Account>> GetAccountInfo()
        {
            string userClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userClaim == null)
                return NotFound();

            Account account = await _context.Accounts.FindAsync(userClaim);
            if (account == null)
                return NotFound();

            return account;
        }

        [HttpPut("ChangeEmail")]
        public async Task<IActionResult> UpdateEmail(string email)
        {
            string userClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userClaim == null)
                return NotFound();

            Account account = await _context.Accounts.FindAsync(userClaim);
            if (account == null)
                return NotFound();

            RegistrationFlags flags = RegistrationFlags.None;
            flags |= AccountValidation.CheckEmail(email);

            if (flags == RegistrationFlags.None)
                flags |= EmailExists(email) ? RegistrationFlags.EmailTaken : RegistrationFlags.None;

            if (flags != RegistrationFlags.None)
                return BadRequest(flags.ToString());

            account.Email = email;
            account.LastEmailUpdate = DateTime.Now;

            _context.Entry(account).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("ChangePassword")]
        public async Task<IActionResult> UpdatePassword(string password)
        {
            string userClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userClaim == null)
                return NotFound();

            Account account = await _context.Accounts.FindAsync(userClaim);
            if (account == null)
                return NotFound();

            RegistrationFlags flags = RegistrationFlags.None;
            flags |= AccountValidation.CheckEmail(password);

            if (flags == RegistrationFlags.None)
                flags |= EmailExists(password) ? RegistrationFlags.EmailTaken : RegistrationFlags.None;

            if (flags != RegistrationFlags.None)
                return BadRequest(flags.ToString());

            account.Email = password;
            account.LastEmailUpdate = DateTime.Now;

            _context.Entry(account).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool EmailExists(string email)
        {
            return (_context.Accounts?.Any(account => account.Email == email)).GetValueOrDefault();
        }
    }
}
