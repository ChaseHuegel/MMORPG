using System.Net.Sockets;
using Mmorpg.Client;
using Mmorpg.Data;
using Swordfish.Library.Networking;
using Swordfish.Library.Threading;
using Swordfish.Library.Util;

namespace Mmorpg.ClientConsole
{
    public class Heartbeat
    {
        public const int TICK_RATE = 15;
        public static GameClient Client;

        private ThreadWorker thread;

        public void Initialize()
        {
            thread = new ThreadWorker(Tick, false, "Heartbeat");
            thread.TargetTickRate = TICK_RATE;

            Start();
            thread.Start();
        }

        public void Start()
        {
            ClientConfig config = Config.Load<ClientConfig>("config/client.toml");
            NetControllerSettings netControllerSettings = new()
            {
                AddressFamily = AddressFamily.InterNetwork,
                DefaultHost = config.Connection.Host,
                KeepAlive = TimeSpan.FromSeconds(10)
            };

            Client = new GameClient(netControllerSettings);
            Client.Disconnected += OnDisconnected;
        }

        public void Stop()
        {
            thread.Stop();
        }

        public void Tick(float deltaTime)
        {
            UpdateTitle();
        }

        private void UpdateTitle()
        {
            Console.Title = $"MMORPG Client | {TICK_RATE}/s | Connected: {Client.SessionCount > 0} | In game: {Client.Session.ID != NetSession.LocalOrUnassigned} | Session: {Client.Session}";
        }

        private void OnDisconnected(object sender, NetEventArgs e)
        {
            Console.WriteLine("Disconnected.");
        }
    }
}
