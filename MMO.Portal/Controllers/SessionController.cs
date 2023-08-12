using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MMO.Bridge.Types;
using MMO.Portal.Data;
using MMO.Portal.Managers;
using MMO.Portal.Models;
using MMO.Portal.Util;
using Swordfish.Library.Util;

namespace MMO.Portal.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SessionController : ControllerBase
    {
        private readonly PortalDbContext _context;
        private readonly UserManager _userManager;

        public SessionController(PortalDbContext context, UserManager userManager)
        {
            _context = context;
            _userManager = userManager;
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
            {
                flags |= LoginFlags.IncorrectUser;
            }
            else
            {
                byte[] hash = Convert.FromBase64String(account.Hash);
                byte[] salt = Convert.FromBase64String(account.Salt);
                byte[] saltedPassword = Security.SaltedHash(Encoding.ASCII.GetBytes(password), salt);

                if (!saltedPassword.SequenceEqual(hash))
                    flags |= LoginFlags.IncorrectPassword;
            }

            if (flags != LoginFlags.None)
                return Unauthorized(flags.ToString());

            var token = await _userManager.SignInAsync(account);

            Console.WriteLine(
                $"User '{user}' logged in. [{HttpContext.Connection.RemoteIpAddress}]"
            );
            return Ok(token);
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            string user = HttpContext.User.GetUserClaim();
            Account account = await _context.Accounts.FindAsync(user);

            await _userManager.SignOutAsync(account);

            Console.WriteLine(
                $"User '{user}' logged out. [{HttpContext.Connection.RemoteIpAddress}]"
            );
            return Ok();
        }

        [HttpPost("Validate")]
        public IActionResult Validate()
        {
            Console.WriteLine(
                $"User '{HttpContext.User.GetUserClaim()}' was validated. [{HttpContext.Connection.RemoteIpAddress}]"
            );
            return Ok();
        }
    }
}
