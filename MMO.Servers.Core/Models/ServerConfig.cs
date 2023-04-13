using Swordfish.Library.Util;

namespace MMO.Servers.Core.Models;

public class ServerConfig : Config
{
    public readonly ConnectionSettings Connection = new();

    public class ConnectionSettings
    {
        public string Address = "localhost";
        public int Port = 42420;
        public int MaxSessions = 300;
        public TimeSpan SessionExpiration = TimeSpan.FromSeconds(30);
    }

    public readonly RegistrationSettings Registration = new();

    public class RegistrationSettings
    {
        public string Name = "";
        public string Type = "";
    }

    public readonly AuthenticationSettings Authentication = new();

    public class AuthenticationSettings
    {
        public string User = "";
        public string Password = "";
    }
}
