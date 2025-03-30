using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Utils
{
    public class VariableParser
    {
        /// <summary>
        /// Parses an Int Environment variable and returns its value.
        /// </summary>
        public static string GetEnvString(string key)
        {
            var value = Environment.GetEnvironmentVariable(key);
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(value.Trim()))
            {
                throw new ArgumentNullException($"Environment variable {key} is not set.");
            }
            return value;
        }

        /// <summary>
        /// Parses an Int Environment variable and returns its value as an integer.
        /// /// </summary>
        public static int GetEnvInt(string key)
        {
            var value = GetEnvString(key);
            if (!int.TryParse(value, out int result))
                throw new InvalidOperationException($"{key} must be a valid integer.");
            return result;
        }
    }
}