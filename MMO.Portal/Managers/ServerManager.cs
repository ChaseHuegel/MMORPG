using System.Collections.Concurrent;
using MMO.Portal.Models;

namespace MMO.Portal.Managers;

public class ServerManager
{
    public readonly ConcurrentBag<Server> Servers = new();
}