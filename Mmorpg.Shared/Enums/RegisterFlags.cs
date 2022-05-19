using System;

namespace Mmorpg.Enums
{
    [Flags]
    public enum RegisterFlags
    {
        None = 0,

        UsernameTaken = 1,
        UsernameInvalidLength = 2,
        UsernameInvalidFormat = 4,

        PasswordInvalidLength = 8,
        PasswordInvalidFormat = 16,

        EmailTaken = 32,
        EmailInvalidFormat = 64
    }
}
