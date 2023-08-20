using System;

namespace MMO.Client.Types
{
    public struct ChatEventArgs
    {
        public string Text { get; private set; }
        
        public ChatEventArgs(string text)
        {
            Text = text;
        }
    }
}