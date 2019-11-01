using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace AuthenticationService.Application.Service
{
    public class KeyHasherService
    {        
        private HashAlgorithm _algorithm;

        public KeyHasherService(HashAlgorithm algorithm)
        {
            _algorithm = algorithm;
        }

        public string EncriptPassword(string password)
        {
            var encodedValue = Encoding.UTF8.GetBytes(password);
            var encryptedPassword = _algorithm.ComputeHash(encodedValue);

            var sb = new StringBuilder();
            foreach (var caracter in encryptedPassword)
            {
                sb.Append(caracter.ToString("X2"));
            }

            return sb.ToString();
        }

        public bool VerifyPassword(string password, string persistedPassword)
        {
            if (string.IsNullOrEmpty(persistedPassword))
                throw new ArgumentNullException();

            var encryptedPassword = _algorithm.ComputeHash(Encoding.UTF8.GetBytes(password));

            var sb = new StringBuilder();
            foreach (var character in encryptedPassword)
            {
                sb.Append(character.ToString("X2"));
            }

            return sb.ToString() == persistedPassword;
        }
    }
}

