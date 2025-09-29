using RepositoryContracts;
using Entities;
using System;
using System.Threading.Tasks;

namespace CLI.UI.Users
{
    public class LoginView
    {
        private readonly IUserRepository _userRepo;

        public LoginView(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        // Return the logged-in user (or null if login failed / canceled)
        public async Task<User?> ShowAsync()
        {
            Console.Clear();
            Console.WriteLine("=== LOGIN ===");
            Console.Write("Username (or blank to cancel): ");
            string? username = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(username)) return null;

            Console.Write("Password: ");
            string? password = Console.ReadLine(); // simple CLI, not hiding

            var user = await _userRepo.GetByUsernameAsync(username);
            if (user == null)
            {
                Console.WriteLine("No such user. Press any key...");
                Console.ReadKey();
                return null;
            }

            // If you store hashed passwords adjust check accordingly
            if (user.Password != password)
            {
                Console.WriteLine("Incorrect password. Press any key...");
                Console.ReadKey();
                return null;
            }

            Console.WriteLine($"Welcome, {user.Username}! Press any key...");
            Console.ReadKey();
            return user;
        }
    }
}