using System.Net;

using Swordfish.Library.Networking;
using Swordfish.Library.Util;

namespace Mmorpg.Client
{
    public class ClientConfig : Config
    {
        public ConnectionSettings Connection = new ConnectionSettings();
        public class ConnectionSettings
        {
            public Host Host => new Host
            {
                Address = IPAddress.TryParse(Address, out IPAddress address) ? address : IPAddress.None,
                Hostname = Hostname,
                Port = Port
            };

            public string Hostname = "localhost";

            public string Address = string.Empty;

            public int Port = 42420;
        }
    }
}
