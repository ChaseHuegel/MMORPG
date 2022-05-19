using Swordfish.Library.Networking;
using Swordfish.Library.Threading;

namespace MMORPG.Server.App
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
            Server = new GameServer();
            
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
            Console.Title = $"MMORPG Server | {Server?.Session.EndPoint} | {TICK_RATE}/s | {Server?.SessionCount}/{Server?.MaxSessions}";
        }
    }
}
