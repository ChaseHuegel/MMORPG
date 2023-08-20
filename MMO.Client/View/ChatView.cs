using System.Text;
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
using MMO.Bridge.Packets;
using MMO.Bridge.Types;
using MMO.Client.Types;

using Debugger = Swordfish.Library.Diagnostics.Debugger;
using Path = Swordfish.Library.IO.Path;

namespace MMO.Client.View;

public class ChatView : Plugin
{
    private const ImGuiWindowFlags CANVAS_FLAGS = ImGuiWindowFlags.NoBringToFrontOnFocus | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar;

    public override string Name => "Chat View";
    public override string Description => "View component of chat capabilities.";

    public EventHandler<ChatEventArgs> Submit;

    private readonly object ChatLock = new();
    private readonly List<ChatPacket> Chat = new();

    public override void Start()
    {
        CanvasElement canvas = new("Chat")
        {
            Flags = CANVAS_FLAGS,
            Constraints = new RectConstraints
            {
                X = new RelativeConstraint(0f),
                Y = new RelativeConstraint(0.8f),
                Width = new RelativeConstraint(0.3f),
                Height = new RelativeConstraint(0.2f)
            }
        };

        PanelElement chatScroll = new("ChatScroll")
        {
            Flags = ImGuiWindowFlags.AlwaysVerticalScrollbar,
            AutoScroll = true,
            TitleBar = false,
            Constraints = new RectConstraints
            {
                Width = new FillConstraint(),
                Height = new RelativeConstraint(0.15f)
            }
        };
        canvas.Content.Add(chatScroll);

        LayoutGroup inputGroup = new() {
            Layout = ElementAlignment.HORIZONTAL
        };

        InputTextElement channel = new(string.Empty, 10) {
            Constraints = new RectConstraints() {
                Width = new AbsoluteConstraint(20)
            }
        };

        InputTextElement input = new(string.Empty, 256);
        input.Submit += OnSubmit;

        inputGroup.Content.Add(channel);
        inputGroup.Content.Add(input);
        canvas.Content.Add(inputGroup);

        lock (ChatLock)
        {
            foreach (ChatPacket chat in Chat)
                AppendChat(chat);
        }

        void OnSubmit(object? sender, string text)
        {
            input.Text = string.Empty;
            Submit?.Invoke(this, new ChatEventArgs(text));
        }

        void AppendChat(ChatPacket chat)
        {
            string text;
            if (string.IsNullOrEmpty(chat.Error))
                text = $"[{(ChatChannel)chat.Channel}] {chat.Sender}: {chat.Message}";
            else
                text = $"[{(ChatChannel)chat.Channel}] {chat.Error}";

            chatScroll.Content.Add(new TextElement(text));
        }
    }

    public void Add(ChatPacket chat)
    {
        lock (ChatLock)
        {
            Chat.Add(chat);
        }
    }
}