using Swordfish.Library.Networking;

namespace Mmorpg.Server.App
{
    public static class Commands
    {
        public static void Read()
        {
            Console.WriteLine();
            string input = Console.ReadLine();
            List<string> arguments = input.Split(' ').ToList();
            while (arguments.Count < 10)
                arguments.Add(string.Empty);

            switch (arguments[0].ToLower())
            {
                case "stop":
                    Application.Exit();
                    break;
                case "list":
                case "players":
                case "sessions":
                case "connected":
                    Console.WriteLine("Server: " + string.Join(", ", Heartbeat.Server.GetSessions()));
                    break;
                case "disconnect":
                    GameServer.Instance.Disconnect();
                    break;
                case "stats":
                    Console.WriteLine(Heartbeat.Server.Stats.Record);
                    Heartbeat.Server.Stats.RequestInterval(TimeSpan.FromSeconds(60), intervalReceived);
                    void intervalReceived(NetStatsRecord record)
                    {
                        Console.WriteLine($"Last 30 seconds: {record}");
                    }
                    break;
            }
        }
    }
}
