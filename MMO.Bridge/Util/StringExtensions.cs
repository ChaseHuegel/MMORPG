using System.Net;
using System.Net.Sockets;
using Swordfish.Library.Networking;

namespace MMO.Bridge.Util;

public static class StringExtensions
{
    public static IPEndPoint ParseIPEndPoint(this string str)
    {
        string[] parts = str.Split(':');
        NetUtils.TryGetHostAddress(parts[0], AddressFamily.InterNetwork, out IPAddress ipAddress);
        return new IPEndPoint(ipAddress, int.Parse(parts[1]));
    }
}
