using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MMO.Portal.Managers;
using MMO.Portal.Models;
using MMO.Portal.Util;

namespace MMO.Portal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServersController : ControllerBase
    {
        private readonly ServerManager _serverManager;

        public ServersController(ServerManager serverManager)
        {
            _serverManager = serverManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<Server>> GetServers()
        {
            return _serverManager.Servers.ToArray();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult RegisterServer(string name, string type, string address)
        {
            var server = new Server
            {
                Name = name,
                Type = type,
                Address = address
            };

            _serverManager.Servers.Add(server);

            Console.WriteLine(
                $"Server '{type}:{name}@{address}' was registered by '{HttpContext.User.GetUserClaim()}'. [{HttpContext.Connection.RemoteIpAddress}]"
            );
            return Ok();
        }
    }
}
