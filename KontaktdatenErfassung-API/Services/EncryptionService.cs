using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KontaktdatenErfassung_API.Services
{
    public static class EncryptionService
    {
        /// <summary>
        /// Verschlüsselt ein Passwort
        /// </summary>
        /// <param name="password">Eine Instanz der <see cref="string"/> Klasse mit dem Passwort</param>
        /// <returns>Das verschlüsselte Passwort als <see cref="string"/></returns>
        public static string EncodePassword(string password)
        {
            byte[] salt;

            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];

            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            string savedPasswordHash = Convert.ToBase64String(hashBytes);
            return savedPasswordHash;
        }

        /// <summary>
        /// Vergleicht ein Passwort, mit einem Eingabe Passwort.
        /// </summary>
        /// <param name="passwordHash">Das verschlüsselte Passwort</param>
        /// <param name="inputPassword">Das eingegebene Passwort</param>
        /// <returns>Eine Instanz der <see cref="bool"/> Klasse mit Erfolg oder Misserfolg</returns>
        public static bool CheckPassword(string passwordHash, string inputPassword)
        {
            string NewPasswordHash = passwordHash.Replace(" ", "+");
            byte[] hashBytes = Convert.FromBase64String(NewPasswordHash);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            var pbkdf2 = new Rfc2898DeriveBytes(inputPassword, salt, 10000);

            byte[] hash = pbkdf2.GetBytes(20);

            int ok = 1;
            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                {
                    ok = 0;
                }
            }
            if (ok == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
