using System;
using System.Security;
using System.Text.RegularExpressions;

namespace FileEncryptor.Utils
{
    public static class PasswordHelper
    {
        public static string ReadPassword()
        {
            var password = new SecureString();
            ConsoleKeyInfo key;

            Console.Write("Enter password: ");
            do
            {
                key = Console.ReadKey(intercept: true);
                if (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Backspace)
                {
                    password.AppendChar(key.KeyChar);
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password.RemoveAt(password.Length - 1);
                    Console.Write("\b \b");
                }
            } while (key.Key != ConsoleKey.Enter);

            Console.WriteLine();
            return ConvertToUnsecureString(password);
        }

        public static bool IsPasswordStrong(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            var hasUpperCase = new Regex(@"[A-Z]");
            var hasLowerCase = new Regex(@"[a-z]");
            var hasDigits = new Regex(@"[0-9]");
            var hasSpecialChars = new Regex(@"[!@#$%^&*(),.?""':;{}|<>]");
            var isValidLength = password.Length >= 8;

            return hasUpperCase.IsMatch(password) && hasLowerCase.IsMatch(password) &&
                   hasDigits.IsMatch(password) && hasSpecialChars.IsMatch(password) && isValidLength;
        }

        private static string ConvertToUnsecureString(SecureString secureString)
        {
            IntPtr unmanagedString = System.Runtime.InteropServices.Marshal.SecureStringToGlobalAllocUnicode(secureString);
            string result = System.Runtime.InteropServices.Marshal.PtrToStringUni(unmanagedString);
            System.Runtime.InteropServices.Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            return result;
        }
    }
}