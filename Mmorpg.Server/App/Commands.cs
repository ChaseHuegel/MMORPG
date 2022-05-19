namespace MMORPG.Server.App
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
                case "list": case "players": case "sessions": case "connected":
                    Console.WriteLine("Server: " + string.Join(", ", Heartbeat.Server.GetSessions()));
                    break;
            }
        }
    }
}
