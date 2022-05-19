using System.Text;

using Swordfish.Integrations.SQL;
using Swordfish.Library.Extensions;
using Swordfish.Library.Util;
using Mmorpg.Enums;

namespace MMORPG.Server.Util
{
    public static class Accounts
    {
        public static RegisterFlags ValidateUsername(string username)
        {
            RegisterFlags flags = RegisterFlags.None;

            if (username.Length < RegisterConstants.MINIMUM_USERNAME_LENGTH)
                flags = RegisterFlags.UsernameInvalidLength;
            
            if (!username.IsAlphaNumeric())
                flags = RegisterFlags.UsernameInvalidFormat;

            if (VerifyUsername(username)) 
                flags = RegisterFlags.UsernameTaken;

            return flags;
        }

        public static RegisterFlags ValidatePassword(string password)
        {
            RegisterFlags flags = RegisterFlags.None;

            if (password.Length < RegisterConstants.MINIMUM_PASSWORD_LENGTH)
                flags = RegisterFlags.PasswordInvalidLength;

            return flags;
        }

        public static RegisterFlags ValidateEmail(string email)
        {
            //  Emails must have at least one '@'
            if (email.Where(x => x == '@').Count() != 1)
                return RegisterFlags.EmailInvalidFormat;

            //  Emails must have at least one '.'
            if (!email.Any(x => x == '.'))
                return RegisterFlags.EmailInvalidFormat;

            //  Emails can't end in '.' or '@'
            if (email.Last() == '.' || email.Last() == '@')
                return RegisterFlags.EmailInvalidFormat;

            //  Emails must be alphanumeric, ignoring '.' and '@'
            if (!email.Without('.', '@').IsAlphaNumeric())
                return RegisterFlags.EmailInvalidFormat;

            if (VerifyEmail(email))
                return RegisterFlags.EmailTaken;

            return RegisterFlags.None;
        }

        public static bool VerifyUsername(string username)
        {
            return Database.Query("mmorpg", "127.0.0.1", 1433, 5)
                .GetRecord("registry", "username", username).Exists();
        }

        public static bool VerifyEmail(string email)
        {
            return Database.Query("mmorpg", "127.0.0.1", 1433, 5)
                .GetRecord("registry", "email", email).Exists();
        }

        public static bool VerifyPassword(string username, string password)
        {
            QueryResult record = Database.Query("mmorpg", "127.0.0.1", 1433, 5)
                .GetRecord("registry", "hash,salt", "username", username);
            
            if (!record.Exists())
                return false;

            string hashRaw = record.Table.Rows[0][0].ToString();
            string saltRaw = record.Table.Rows[0][1].ToString();
            byte[] hash = Convert.FromBase64String(hashRaw);
            byte[] salt = Convert.FromBase64String(saltRaw);

            byte[] saltedPassword = Security.SaltedHash(Encoding.ASCII.GetBytes(password), salt);
            return saltedPassword.SequenceEqual(hash);
        }

        public static void Register(string username, string password, string email)
        {
            byte[] salt = Security.Salt(16);
            byte[] hash = Security.SaltedHash(Encoding.ASCII.GetBytes(password), salt);

            //  Register the provided account to the database
            Database.Query("mmorpg", "127.0.0.1", 1433, 5)
                    .InsertInto("registry")
                    .Columns("username", "email", "salt", "hash")
                    .Values(username, email, Convert.ToBase64String(salt), Convert.ToBase64String(hash))
                    .Execute();
        }
    }
}
