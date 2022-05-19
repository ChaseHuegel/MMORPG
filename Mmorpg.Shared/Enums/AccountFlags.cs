using System;

namespace Mmorpg.Enums
{
    [Flags]
    public enum AccountFlags
    {
        None = 0,
        UsernameIncorrect = 1,
        PasswordIncorrect = 2,
        EmailIncorrect = 4,
        AlreadyLoggedIn = 8
    }
}
