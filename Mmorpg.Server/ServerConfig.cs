using Swordfish.Library.Util;

namespace MMORPG.Server
{
    public class ServerConfig : Config
    {
        public ConnectionSettings Connection = new ConnectionSettings();
        public class ConnectionSettings
        {
            public int Port = 42420;
            
            public int MaxPlayers = 300;
        }
    }
}
