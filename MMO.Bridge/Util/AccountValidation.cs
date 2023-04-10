using System.Net.Mail;
using MMO.Bridge.Types;
using Swordfish.Library.Extensions;

namespace MMO.Bridge.Util;

public static class AccountValidation
{
    public const int MIN_USERNAME_LENGTH = 4;
    public const int MIN_PASSWORD_LENGTH = 4;

    public static RegistrationFlags CheckUser(string user)
    {
        var flags = RegistrationFlags.None;

        if (user.Length < MIN_USERNAME_LENGTH)
            flags |= RegistrationFlags.UserInvalidLength;

        if (!char.IsLetter(user[0]))
            flags |= RegistrationFlags.UserInvalidFormat;

        if (!user.IsAlphaNumeric())
            flags |= RegistrationFlags.UserInvalidFormat;

        return flags;
    }

    public static RegistrationFlags CheckPassword(string password)
    {
        var flags = RegistrationFlags.None;

        if (password.Length < MIN_PASSWORD_LENGTH)
            flags |= RegistrationFlags.UserInvalidLength;

        if (password.Contains(' ') || password.Contains('\t'))
            flags |= RegistrationFlags.UserInvalidFormat;

        return flags;
    }

    public static RegistrationFlags CheckEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return RegistrationFlags.EmailInvalidFormat;

        if (email.Last() == '.' || email.Last() == '@')
            return RegistrationFlags.EmailInvalidFormat;

        if (email.Any(c => c == ' ' || c == '\t'))
            return RegistrationFlags.EmailInvalidFormat;

        string[] segments = email.Split('@');

        if (segments.Length != 2)
            return RegistrationFlags.EmailInvalidFormat;

        string address = segments[0];
        string server = segments[1];

        if (!address.Without('.').IsAlphaNumeric())
            return RegistrationFlags.EmailInvalidFormat;

        if (!server.Without('.').IsAlphaNumeric())
            return RegistrationFlags.EmailInvalidFormat;

        try
        {
            var mailAddress = new MailAddress(email);
            if (mailAddress.Address != email)
                return RegistrationFlags.EmailInvalidFormat;
        }
        catch
        {
            //  Do nothing
        }

        return RegistrationFlags.None;
    }
}
