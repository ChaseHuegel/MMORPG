namespace MMO.Bridge.Types;

[Flags]
public enum LoginFlags
{
    None = 0,
    IncorrectUser = 1,
    IncorrectPassword = 2,
    AlreadyLoggedIn = 4,
}
