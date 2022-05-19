using System;

namespace Mmorpg.Enums
{
    [Flags]
    public enum DeleteCharacterFlags
    {
        None = 0,
        NotLoggedIn = 1,
    }
}
