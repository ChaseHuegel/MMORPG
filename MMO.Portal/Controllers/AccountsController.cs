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
        private readonly UserManager _userManager;

        public AccountsController(PortalDbContext context, UserManager userManager)
        {
            _context = context;
            _userManager = userManager;
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
        public async Task<IActionResult> CreateAccount(string user, string password, string email, string roles = null)
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
                Hash = Convert.ToBase64String(hash),
                Roles = roles
            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            //  Sign in on account creation
            await _userManager.SignInAsync(HttpContext, account, false);

            return Ok();
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string user, string password)
        {
            if (User.Identity.IsAuthenticated)
                return Unauthorized(LoginFlags.AlreadyLoggedIn.ToString());

            LoginFlags flags = LoginFlags.None;
            Account account = await _context.Accounts.FindAsync(user);
            if (account == null)
                flags |= LoginFlags.IncorrectUser;

            byte[] hash = Convert.FromBase64String(account.Hash);
            byte[] salt = Convert.FromBase64String(account.Salt);
            byte[] saltedPassword = Security.SaltedHash(Encoding.ASCII.GetBytes(password), salt);

            if (!saltedPassword.SequenceEqual(hash))
                flags |= LoginFlags.IncorrectPassword;

            if (flags != LoginFlags.None)
                return Unauthorized(flags.ToString());

            await _userManager.SignInAsync(HttpContext, account, false);

            return Ok();
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _userManager.SignOutAsync(HttpContext);
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
