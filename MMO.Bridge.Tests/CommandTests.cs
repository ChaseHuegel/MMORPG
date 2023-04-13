using System;
using System.Threading.Tasks;
using MMO.Bridge.Commands;
using MMO.Bridge.Types;
using NUnit.Framework;

namespace MMO.Bridge.Tests;

public class Tests
{
    private readonly CommandParser CommandParser = new(new HomeCommand());

    [Test]
    public void TestHelpTextOutput()
    {
        Console.WriteLine();
        Console.WriteLine(CommandParser.GetHelpText());
    }

    [Test]
    [TestCase("/home set myhome", ExpectedResult = true)]
    [TestCase("home set myhome", ExpectedResult = false)]
    [TestCase("this isn't a command", ExpectedResult = false)]
    public async Task<bool> TestCommand(string command)
    {
        return await CommandParser.TryRunAsync(command);
    }

    public class HomeCommand : Command<HomeTpCommand, HomeSetCommand, HomeDeleteCommand>
    {
        public override string Option => "home";
        public override string Description => "Manage your home teleports.";
        public override string ArgumentsHint => "";
    }

    public class HomeSetCommand : Command
    {
        public override string Option => "set";
        public override string Description => "Sets a home teleport.";
        public override string ArgumentsHint => "<name>";

        protected override Task<CommandCompletion> InvokeAsync(ReadOnlyQueue<string> args)
        {
            string name = args.Take();
            Console.WriteLine($"Home set to {name}");
            return Task.FromResult(CommandCompletion.Success);
        }
    }

    public class HomeTpCommand : Command
    {
        public override string Option => "tp";
        public override string Description => "Teleports to one of your homes.";
        public override string ArgumentsHint => "<name>";

        protected override Task<CommandCompletion> InvokeAsync(ReadOnlyQueue<string> args)
        {
            string name = args.Take();
            Console.WriteLine($"Home tp to {name}");
            return Task.FromResult(CommandCompletion.Success);
        }
    }

    public class HomeDeleteCommand : Command
    {
        public override string Option => "delete";
        public override string Description => "Delete home teleports.";
        public override string ArgumentsHint => "(<name>|all)";

        protected override Task<CommandCompletion> InvokeAsync(ReadOnlyQueue<string> args)
        {
            string arg1 = args.Take();

            if (arg1.Equals("all", StringComparison.OrdinalIgnoreCase))
                Console.WriteLine($"Home delete all");
            else
                Console.WriteLine($"Home delete {arg1}");

            return Task.FromResult(CommandCompletion.Success);
        }
    }
}
