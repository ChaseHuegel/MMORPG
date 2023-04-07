using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MMO.Bridge.Types;
using MMO.Portal.Data;
using MMO.Portal.Managers;
using MMO.Portal.Models;
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

        [HttpPost("Validate")]
        public IActionResult Validate()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);
            HttpContext.User.WriteTo(writer);
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer);
            var encodedPrincipal = Convert.ToBase64String(buffer);
            return Content(string.Join(';', HttpContext.User.Claims.Select(x => x.ToString())));
        }

        [HttpPost("Authorize")]
        [AllowAnonymous]
        public IActionResult Authorize(string user)
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized();

            string userInCookie = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            if (userInCookie != user)
                return Unauthorized();

            return Ok();
        }
    }
}
