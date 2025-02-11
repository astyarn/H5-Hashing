using System.Text;
using System.Security.Cryptography;

namespace H5_Hashing
{
    public class Program
    {
        static string FilePath { get; set; } = "users.txt";

        static void Main(string[] args)
        {
            MenuDisplay();
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddUser();
                    break;
                case "2":
                    ShowUsers();
                    break;
                case "3":
                    Console.WriteLine("Which User do you want to Delete?");
                    var (usernameDel, passwordDel) = GetUserInput();
                    if (TestLogin(usernameDel, passwordDel))
                    {
                        Console.WriteLine("Correct Username and Password match!");
                        DeleteUser(usernameDel);
                    }
                    else
                    {
                        Console.WriteLine("Incorrect Username and Password match!");
                    }
                    return;
                case "4":
                    var (username, password) = GetUserInput();
                    if (TestLogin(username, password))
                    {
                        Console.WriteLine("Correct Username and Password match!");
                    }
                    else
                    {
                        Console.WriteLine("Incorrect Username and Password match!");
                    }
                    return;
                default:
                    Console.WriteLine("Invalid choice, try again.");
                    break;
            }
        }

        private static void MenuDisplay()
        {
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine(" Add user            : 1");
            Console.WriteLine(" Show user list      : 2");
            Console.WriteLine(" Delete user         : 3");
            Console.WriteLine(" Login as user       : 4");
            Console.WriteLine(" Choose an option    : ");
        }

        static (string, string) GetUserInput()
        {
            Console.Write("Enter username: ");
            string username = Console.ReadLine();
            Console.Write("Enter password: ");
            string password = Console.ReadLine();
            return (username, password);
        }

        static void AddUser()
        {
            var (username, password) = GetUserInput();

            string hashedPassword = HashPassword(password); //hash can take empty string

            string entry = $"{username}:{hashedPassword}";    

            File.AppendAllText(FilePath, entry + Environment.NewLine);
            Console.WriteLine("User added successfully.\n");
        }

        public static void DeleteUser(string username, string filePath = "users.txt")
        {
            // Check if the file exists
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File does not exist.");
                return;
            }

            var lines = File.ReadAllLines(filePath).ToList();

            var lineToRemove = lines.FirstOrDefault(line => line.Split(':')[0] == username);

            if (lineToRemove != null)
            {
                // Remove the line with the matching username
                lines.Remove(lineToRemove);

                File.WriteAllLines(filePath, lines);

                Console.WriteLine($"User {username} has been deleted.");
            }
            else
            {
                Console.WriteLine($"User {username} not found.");
            }
        }

        static void ShowUsers()
        {
            if (File.Exists(FilePath))
            {
                string[] users = File.ReadAllLines(FilePath);
                Console.WriteLine("\nStored Users:");
                foreach (var user in users)
                {
                    Console.WriteLine(user);
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("No users found.\n");
            }
        }

        public static string HashPassword(string password)
        {
            //Able to handle empty string
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToHexString(bytes);  // Returns a hex string
            }
        }

        public static bool TestLogin(string user, string pw, string filePath = "users.txt")
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("file with users not found.");
                return false;
            }

            string[] users = File.ReadAllLines(filePath);

            foreach (string line in users)
            {
                string[] parts = line.Split(':');
                if (parts.Length == 2)
                {
                    string storedUsername = parts[0];
                    string storedHashedPassword = parts[1];

                    if (storedUsername == user)
                    {
                        string enteredHashedPassword = HashPassword(pw);
                        return enteredHashedPassword == storedHashedPassword;
                    }
                }
            }

            return false;
        }

    }
}
