using Swordfish.Library.Util;

namespace MMO.Client.Models;

public class ClientConfig : Config
{
    public readonly ConnectionSettings Connection = new();

    public class ConnectionSettings
    {
        public string PortalUrl = "https://localhost:7297";
    }
}
