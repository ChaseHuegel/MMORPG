using Microsoft.AspNetCore.Mvc;
using MMO.Bridge.Models;
using MMO.DataServer.Data;

namespace MMO.DataServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharactersController : ControllerBase
    {
        private readonly CharactersDbContext _context;

        public CharactersController(CharactersDbContext context)
        {
            _context = context;
        }

        [HttpGet("CreationRules")]
        public ActionResult<CharacterCreationRules> GetClasses()
        {
            var rules = new CharacterCreationRules
            {
                MinNameLength = 4,
                MaxNameLength = 20,
                AllowedNameChars = "abcdefghijklmnopqrstuvwxyz ",
                Races = _context.Races.ToArray(),
                Classes = _context.Classes.ToArray(),
                AllowedCombinations = new Dictionary<int, int[]>
                {
                    { 0, new int[] { 0, 1, 2, 3 } },
                    { 1, new int[] { 1, 2, 3 } },
                    { 2, new int[] { 1, 3 } },
                }
            };

            return rules;
        }
    }
}
