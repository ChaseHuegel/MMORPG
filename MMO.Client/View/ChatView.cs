using ImGuiNET;
using Swordfish.Extensibility;
using Swordfish.Library.Constraints;
using Swordfish.Types.Constraints;
using Swordfish.UI.Elements;
using MMO.Bridge.Packets;
using MMO.Client.Types;

using Debugger = Swordfish.Library.Diagnostics.Debugger;
using Swordfish.Library.IO;

namespace MMO.Client.View;

public class ChatView : Plugin
{
    private const ImGuiWindowFlags CANVAS_FLAGS = ImGuiWindowFlags.NoBringToFrontOnFocus | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar;

    private readonly IShortcutService ShortcutService;

    private EventHandler<ChatPacket>? NewChat;

    private readonly object ChatLock = new();
    private readonly List<ChatPacket> Chat = new();

    public EventHandler<ChatEventArgs>? Submit;

    public override string Name => "Chat View";
    public override string Description => "View component of chat capabilities.";

    public ChatView(IShortcutService shortcutService)
    {
        ShortcutService = shortcutService;
        Submit = null;
        NewChat = null;
    }

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
        NewChat += OnNewChat;

        LayoutGroup inputGroup = new() {
            Layout = ElementAlignment.HORIZONTAL
        };

        InputTextElement input = new(string.Empty, 256);
        input.Submit += OnSubmit;

        ShortcutService.RegisterShortcut(
            new Shortcut(
                "Chat",
                "UI",
                ShortcutModifiers.NONE,
                Key.ENTER,
                Shortcut.DefaultEnabled,
                FocusInput
            )
        );

        ShortcutService.RegisterShortcut(
            new Shortcut(
                "Command",
                "UI",
                ShortcutModifiers.NONE,
                Key.SLASH,
                Shortcut.DefaultEnabled,
                StartCommandInput
            )
        );

        inputGroup.Content.Add(input);
        canvas.Content.Add(inputGroup);

        lock (ChatLock)
        {
            foreach (ChatPacket chat in Chat)
                AppendChat(chat);
        }

        void OnNewChat(object? sender, ChatPacket chat)
        {
            AppendChat(chat);
        }

        void OnSubmit(object? sender, string text)
        {
            if (!string.IsNullOrEmpty(input.Text))
                Submit?.Invoke(this, new ChatEventArgs(text));

            input.Text = string.Empty;
        }

        void AppendChat(ChatPacket chat)
        {
            chatScroll.Content.Add(new TextElement(chat.ToString()));
        }

        void FocusInput()
        {
            if (string.IsNullOrEmpty(input.Text))
                input.Focus();
        }

        void StartCommandInput()
        {
            FocusInput();
            input.Text = "/";
        }
    }

    public void Add(ChatPacket chat)
    {
        lock (ChatLock)
        {
            Debugger.Log($"Chat: {chat.Message}");
            Chat.Add(chat);
            NewChat?.Invoke(this, chat);
        }
    }
}