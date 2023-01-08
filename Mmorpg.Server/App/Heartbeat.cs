using System.Net.Sockets;
using Swordfish.Library.Networking;
using Swordfish.Library.Threading;
using Swordfish.Library.Util;

namespace Mmorpg.Server.App
{
    public class Heartbeat
    {
        public const int TICK_RATE = 15;
        public static GameServer Server;

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
            ServerConfig config = Config.Load<ServerConfig>("config/server.toml");
            NetControllerSettings netControllerSettings = new()
            {
                AddressFamily = AddressFamily.InterNetwork,
                Port = config.Connection.Port,
                MaxSessions = config.Connection.MaxPlayers,
                SessionExpiration = config.Connection.SessionExpiration
            };

            Server = new GameServer(netControllerSettings);
        }

        public void Stop()
        {
            thread.Stop();
        }

        public void Tick(float deltaTime)
        {
            Server.Tick(deltaTime);
            UpdateTitle();
        }

        private void UpdateTitle()
        {
            Console.Title = $"MMORPG Server | {Server?.Session.EndPoint} | {TICK_RATE}/s | {Server?.SessionCount}/{Server?.MaxSessions}";
        }
    }
}
