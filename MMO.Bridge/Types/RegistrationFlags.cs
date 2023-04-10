namespace MMO.Bridge.Types;

[Flags]
public enum RegistrationFlags
{
    None = 0,

    UserTaken = 1,
    UserInvalidLength = 2,
    UserInvalidFormat = 4,

    PasswordInvalidLength = 8,
    PasswordInvalidFormat = 16,

    EmailTaken = 32,
    EmailInvalidFormat = 64
}
