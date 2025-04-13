using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace api.Utils
{
    public class RandomCharGenerator
    {
        private const string AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*";

        /*
        * Generates a random string of the specified length using the allowed characters.
        * The default length is 10 characters.
        */
        public static string GenerateRandomPassword(int length = 10)
        {
            return new string([.. Enumerable.Range(0, length).Select(_ => AllowedChars[RandomNumberGenerator.GetInt32(AllowedChars.Length)])]);
        }

    }
}