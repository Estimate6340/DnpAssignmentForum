using RepositoryContracts;
using Entities;

namespace CLI.UI.Users;

public class LoginView
{
    private readonly IUserRepository _userRepo;
    private User? _currentUser;

    public LoginView(IUserRepository userRepo, ref User? currentUser)
    {
        _userRepo = userRepo;
        _currentUser = currentUser;
    }

    public async Task ShowAsync()
    {
        Console.Write("Username: ");
        var username = Console.ReadLine() ?? "";
        Console.Write("Password: ");
        var password = Console.ReadLine() ?? "";

        var user = await _userRepo.GetByUsernameAsync(username);
        if (user != null && user.Password == password)
        {
            _currentUser = user;
            Console.WriteLine($"Logged in as {user.Username}");
        }
        else
        {
            Console.WriteLine("Invalid login.");
        }
        Console.ReadKey();
    }
}