using System.Collections.Concurrent;
namespace MMO.Portal.Models;

public class ServerManager
{
    public readonly ConcurrentBag<Server> Servers = new();
}