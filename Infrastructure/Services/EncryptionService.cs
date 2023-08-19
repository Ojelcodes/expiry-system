using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Application.ConfigSettings;
using Microsoft.Extensions.Options;
using Application.InfraInterfaces;

namespace Infrastructure.Services
{
    public class EncryptionService : IEncryptionService
    {
        private readonly ILogger<EncryptionService> _logger;

        public EncryptionService(ILogger<EncryptionService> logger)
        {
            _logger = logger;
        }

        private static bool IsBase64String(string base64String)
        {
            base64String = base64String.Trim();
            return base64String.Length % 4 == 0 &&
               Regex.IsMatch(base64String, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
        }

        public string SHA512(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);
                // Convert to text
                // StringBuilder Capacity is 128, because 512 bits / 8 bits in byte * 2 symbols for byte
                var hashedInputStringBuilder = new StringBuilder(128);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("x2"));
                return hashedInputStringBuilder.ToString();
            }
        }
    }
}
