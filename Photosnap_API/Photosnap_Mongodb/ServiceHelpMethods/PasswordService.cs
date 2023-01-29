using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Photosnap_Mongodb.ServiceHelpMethods
{
    public static class PasswordService
    {
        public static byte[] EncryptPassword(string password, byte[] passwordSalt)
        {
            byte[] hashedPassword;
            byte[] originalPasswordInBytes;
            if (string.IsNullOrEmpty(password) || passwordSalt.Length == 0)
                return new byte[0];

            originalPasswordInBytes = Encoding.ASCII.GetBytes(password);

            int totalLengthCombined = (originalPasswordInBytes.Length + passwordSalt.Length);

            hashedPassword = new byte[totalLengthCombined];
            int i;
            for (i = 0; i < passwordSalt.Length - 1; i++)
            {
                hashedPassword[i] = passwordSalt[i];
            }
            for (int j = 0; j < originalPasswordInBytes.Length - 1; j++)
            {
                hashedPassword[i] = originalPasswordInBytes[j];
                i++;
            }
            return HashPassword(hashedPassword);
        }

        public static byte[] HashPassword(byte[] password)
        {
            SHA256 _sha256 = SHA256.Create();
            return _sha256.ComputeHash(password);
        }
    }
}
