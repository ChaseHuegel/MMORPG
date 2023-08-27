using System.Net.Mime;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using ImGuiNET;
using Swordfish.ECS;
using Swordfish.Extensibility;
using Swordfish.Graphics;
using Swordfish.Library.Constraints;
using Swordfish.Library.Diagnostics;
using Swordfish.Library.Extensions;
using Swordfish.Library.IO;
using Swordfish.Library.Reflection;
using Swordfish.Library.Types;
using Swordfish.Types.Constraints;
using Swordfish.UI.Elements;
using MMO.Client.View;
using MMO.Bridge.Packets;
using MMO.Bridge.Types;
using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;
using MMO.Client.Types;

using Debugger = Swordfish.Library.Diagnostics.Debugger;
using Path = Swordfish.Library.IO.Path;

namespace MMO.Client.Control;

public class ChatController : Plugin
{
    public override string Name => "Chat Controller";
    public override string Description => "Control component of chat capabilities.";

    private static ClientController ClientController;
    private static ChatView ChatView;
    private readonly IShortcutService ShortcutService;

    public ChatController(ClientController clientController, ChatView chatView, IShortcutService shortcutService)
    {
        ClientController = clientController;
        ChatView = chatView;
        ShortcutService = shortcutService;
    }

    public override void Start()
    {
        ChatView.Submit += OnChatSubmitted;
    }

    private void OnChatSubmitted(object sender, ChatEventArgs args)
    {
        ClientController.CommandParser.TryRunAsync(args.Text).ContinueWith(OnUnknownCommand);
    }

    private void OnUnknownCommand(Task<CommandResult> task)
    {
        if (task.Result.IsSuccessState)
            return;

        ChatPacket chat = new() {
            Channel = (int)ChatChannel.System,
            Error = $"Unknown command: \"{task.Result.OriginalString}\"."
        };

        ChatView.Add(chat);
    }

    [ClientPacketHandler]
    public static void OnChatReceived(NetClient client, ChatPacket packet, NetEventArgs e)
    {
        ChatView.Add(packet);
    }
}