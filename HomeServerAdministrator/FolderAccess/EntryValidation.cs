using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace HomeServerAdministrator
{

    ///<summary>
    /// Container class with static methods for Folder Creation and Deletion entry validation
    /// </summary>
    class EntryValidation
    {
        // Constants for data validation
        private const int NameMaxLength = 25;
        private const int NameMinLength = 2;

        private const int MaxEmailLength = 100;
        private const string EmailPattern = "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$";

        private const int MaxPasswordLength = 20;
        private const int MinPasswordLength = 8;

        public static void ValidateName(string name)
        {
            // Ensure valid length
            if (name.Length > NameMaxLength || name.Length < NameMinLength)
            {
                throw new FolderNameException($"Name must be between {NameMinLength} and {NameMaxLength} characters.");
            }

            // Ensure no white space
            if (ContainsSpaces(name))
            {
                throw new FolderNameException("Name cannot contain spaces.");
            }
        }

        public static void ValidateEmail(string email)
        {
            // Ensure email length
            if (email.Length > MaxEmailLength)
            {
                throw new FolderEmailException($"Email must not exceed {MaxEmailLength} characters.");
            }

            // Ensure email patten
            if (!Regex.IsMatch(email, EmailPattern))
            {
                throw new FolderEmailException("Email must follow the format: 'username@example.com'");
            }

        }

        public static void ValidatePassword(string password)
        {
            // Ensure password length
            if (password.Length > MaxPasswordLength || password.Length < MinPasswordLength)
            {
                throw new FolderPasswordException($"Password must be between {MaxPasswordLength} and {MinPasswordLength} characters.");
            }

            // Ensure no whie space
            if (ContainsSpaces(password))
            {
                throw new FolderPasswordException("Password cannot contain spaces.");
            }

        }


        public static bool IsAdminPasswordValid(string password)
        {
            if (password == "TemporaryPassword")
            {
                return true;
            }
            return false;
        }

        private static bool ContainsSpaces(string str)
        {
            foreach (char character in str)
            {
                if (character == ' ')
                {
                    return true;
                }
            }
            return false;
        }
    }
}
