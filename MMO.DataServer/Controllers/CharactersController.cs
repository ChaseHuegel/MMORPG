using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MMO.DataServer.Data;

namespace MMO.DataServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CharactersController : ControllerBase
    {
        private readonly CharactersDbContext _context;

        public CharactersController(CharactersDbContext context)
        {
            _context = context;
        }

        [HttpGet("Classes")]
        public ActionResult<IEnumerable<string>> GetClasses()
        {
            return new string[] {
                "Warrior",
                "Wizard",
                "Cleric",
                "Rogue",
            };
        }
    }
}