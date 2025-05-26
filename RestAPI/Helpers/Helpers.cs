using System.Text.RegularExpressions;
using BCrypt.Net;

namespace RestAPI.Helpers
{
    public static class PasswordHelper
    {
        private static readonly Regex PasswordPattern = new Regex(
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+{}|;:,.<>?]).{8,}$");

        public static bool IsPasswordValid(string password, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (string.IsNullOrEmpty(password))
            {
                errorMessage = "Password cannot be empty.";
                return false;
            }
            if (!PasswordPattern.IsMatch(password))
            {
                errorMessage = "Password must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter, one digit, and one special character.";
                return false;
            }

            return true;
        }

        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }

}
