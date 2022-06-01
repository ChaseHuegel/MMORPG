using System;

namespace Mmorpg.Enums
{
    [Flags]
    public enum CreateCharacterFlags
    {
        None = 0,
        NameTaken = 1,
        NameInvalidLength = 2,
        NameInvalidFormat = 4,
        NoOpenSlot = 8,
        InvalidRequest = 16,
        NotLoggedIn = 32,
        InvalidCombination = 64,
    }
}
