using Swordfish.Library.Networking;
using Mmorpg.Client;
using Mmorpg.Packets;

namespace Mmorpg.ClientConsole
{
    public class Application
    {
        private static bool stop = false;

        private static Heartbeat heartbeat = new Heartbeat();

        static void Main(string[] args)
        {
            PacketManager.RegisterAssembly<CharacterListPacket>();
            PacketManager.RegisterAssembly<GameClient>();

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
