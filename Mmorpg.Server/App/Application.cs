using System.Reflection;
using Swordfish.Library.Networking;
using Mmorpg.Packets;

namespace MMORPG.Server.App
{
    public class Application
    {
        private static bool stop = false;

        private static Heartbeat heartbeat = new Heartbeat();

        static void Main(string[] args)
        {
            PacketManager.RegisterAssembly<CharacterListPacket>();
            PacketManager.RegisterAssembly<GameServer>();

            try
            {
                heartbeat.Initialize();

                while (!stop)
                    Commands.Read();

                heartbeat.Stop();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static void Exit()
        {
            stop = true;
        }
    }
}
