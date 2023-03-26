using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MMO.Bridge.Types;
using MMO.Bridge.Util;
using MMO.Portal.Models;
using Swordfish.Library.Util;

namespace MMO.Portal.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly PortalDbContext _context;

        public AccountsController(PortalDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {
            var accountList = await _context.Accounts.ToListAsync();
            return accountList;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAccount(string user, string password, string email)
        {
            RegistrationFlags flags = RegistrationFlags.None;
            flags |= AccountValidation.CheckUser(user);
            flags |= AccountValidation.CheckPassword(password);
            flags |= AccountValidation.CheckEmail(email);

            if (flags == RegistrationFlags.None)
            {
                flags |= UserExists(user) ? RegistrationFlags.UserTaken : RegistrationFlags.None;
                flags |= EmailExists(email) ? RegistrationFlags.EmailTaken : RegistrationFlags.None;
            }

            if (flags != RegistrationFlags.None)
                return BadRequest(flags.ToString());

            byte[] salt = Security.Salt(16);
            byte[] hash = Security.SaltedHash(Encoding.ASCII.GetBytes(password), salt);

            var account = new Account
            {
                User = user,
                Email = email,
                Salt = Convert.ToBase64String(salt),
                Hash = Convert.ToBase64String(hash)
            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{user}")]
        public async Task<IActionResult> DeleteAccount(string user)
        {
            ClaimsPrincipal currentUser = User;
            bool isAdmin = currentUser.IsInRole("admin");

            if (currentUser.Identity.Name != user && !isAdmin)
                return Unauthorized();

            Account account = await _context.Accounts.FindAsync(user);
            if (account == null)
                return NotFound();

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{user}")]
        public async Task<IActionResult> UpdateAccount(string user, string email, string password)
        {
            ClaimsPrincipal currentUser = User;
            bool isAdmin = currentUser.IsInRole("admin");

            if (currentUser.Identity.Name != user && !isAdmin)
                return Unauthorized();

            Account account = await _context.Accounts.FindAsync(user);
            if (account == null)
                return NotFound();

            RegistrationFlags flags = RegistrationFlags.None;
            flags |= AccountValidation.CheckPassword(password);
            flags |= AccountValidation.CheckEmail(email);

            if (flags == RegistrationFlags.None)
            {
                flags |= UserExists(user) ? RegistrationFlags.UserTaken : RegistrationFlags.None;
                flags |= EmailExists(email) ? RegistrationFlags.EmailTaken : RegistrationFlags.None;
            }

            if (flags != RegistrationFlags.None)
                return BadRequest(flags.ToString());

            account.Email = email;
            account.LastEmailUpdate = DateTime.Now;

            _context.Entry(account).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool UserExists(string user)
        {
            return (_context.Accounts?.Any(account => account.User == user)).GetValueOrDefault();
        }

        private bool EmailExists(string email)
        {
            return (_context.Accounts?.Any(account => account.Email == email)).GetValueOrDefault();
        }
    }
}
