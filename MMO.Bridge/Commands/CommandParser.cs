using System.Text;
using MMO.Bridge.Types;

namespace MMO.Bridge.Commands;

public class CommandParser
{
    private readonly List<Command> Commands;
    private readonly char Indicator;

    public CommandParser(params Command[] commands)
    {
        Commands = new List<Command>(commands);
    }

    public CommandParser(char indicator, params Command[] commands)
        : this(commands)
    {
        Indicator = indicator;
    }

    public string GetHelpText()
    {
        var builder = new StringBuilder();
        foreach (Command command in Commands)
            builder.AppendLine(Indicator + command.GetHelpText());

        return builder.ToString();
    }

    public async Task<bool> TryRunAsync(string? line)
    {
        if (string.IsNullOrWhiteSpace(line))
            return false;

        if (Indicator != default && !line.StartsWith(Indicator))
            return false;

        var lineStart = Indicator != default ? 1 : 0;
        var parts = line[lineStart..].Split(
            ' ',
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
        );

        var args = new ReadOnlyQueue<string>(parts);

        foreach (Command command in Commands)
        {
            if (await command.TryInvokeAsync(args))
                return true;
        }

        return false;
    }
}
