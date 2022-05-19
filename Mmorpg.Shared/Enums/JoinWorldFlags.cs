using System;

namespace Mmorpg.Enums
{
    [Flags]
    public enum JoinWorldFlags
    {
        None = 0,
        NotLoggedIn = 1,
        JoinFailed = 2,
    }
}
