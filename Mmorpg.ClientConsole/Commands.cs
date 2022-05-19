using MMORPG.Server.Util;
using Swordfish.Library.Types;
using Mmorpg.Data;
using Mmorpg.Enums;
using Mmorpg.Packets;

namespace Mmorpg.ClientConsole
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
                case "sessions":
                    Console.WriteLine("Client: " + string.Join(", ", Heartbeat.Client.GetSessions()));
                    break;
                case "say":
                    Heartbeat.Client.Send(new ChatPacket {
                        Message = string.Join(' ', arguments.Skip(1))
                    });
                    break;
                case "register":
                    Heartbeat.Client.Send(new RegisterPacket {
                        Username = arguments[1],
                        Email = arguments[2],
                        Password = arguments[3]
                    });
                    break;
                case "login":
                    Heartbeat.Client.Send(new LoginPacket {
                        Username = arguments[1],
                        Password = arguments[2]
                    });
                    break;
                case "characters":
                    Heartbeat.Client.Send(new CharacterListPacket {});
                    break;
                case "enter":
                    Heartbeat.Client.Send(new JoinWorldPacket {
                        Slot = Int32.Parse(arguments[1])
                    });
                    break;
                case "delete":
                    Heartbeat.Client.Send(new DeleteCharacterPacket {
                        Slot = Int32.Parse(arguments[1])
                    });
                    break;
                case "create":
                    Console.WriteLine("-- Character Creation --");
                    Console.WriteLine("Type 'cancel' to exit");
                    Console.WriteLine();

                    Console.WriteLine("Races: " + string.Join(", ", Characters.GetAllRaces().Select(x => x.Name)));
                    Console.WriteLine("Pick a race: ");
                    input = Console.ReadLine();
                    DynamicEnumValue chosenRace = CharacterRaces.Get(input);
                    Console.WriteLine();
                    if (input.ToLower() == "cancel") return;

                    while (chosenRace == null)
                    {
                        Console.WriteLine("Invalid race! Try again...");
                        Console.WriteLine("Pick a race: ");
                        input = Console.ReadLine();
                        chosenRace = CharacterRaces.Get(input);
                        Console.WriteLine();

                        if (input.ToLower() == "cancel") return;
                    }

                    Console.WriteLine("Classes: " + string.Join(", ", Characters.GetValidClasses(chosenRace).Select(x => x.Name)));
                    Console.WriteLine("Pick a class: ");
                    input = Console.ReadLine();
                    DynamicEnumValue chosenClass = CharacterClasses.Get(input);
                    Console.WriteLine();
                    if (input.ToLower() == "cancel") return;

                    while (Characters.ValidateRaceClass(chosenRace, chosenClass) != CreateCharacterFlags.None)
                    {
                        Console.WriteLine("Invalid class! Try again...");
                        Console.WriteLine("Pick a class: ");
                        input = Console.ReadLine();
                        chosenClass = CharacterClasses.Get(input);
                        Console.WriteLine();

                        if (input.ToLower() == "cancel") return;
                    }

                    Console.WriteLine("Pick your name: ");
                    input = Console.ReadLine();
                    string chosenName = input;
                    Console.WriteLine();
                    if (input.ToLower() == "cancel") return;

                    Heartbeat.Client.Send(new CreateCharacterPacket {
                        Name = chosenName,
                        Race = chosenRace,
                        Class = chosenClass,
                    });
                    break;
            }
        }
    }
}
