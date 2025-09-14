using RepositoryContracts;
using Entities;

namespace CLI.UI.Users;

public class CreateUserView
{
    private readonly IUserRepository _userRepo;

    public CreateUserView(IUserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    public async Task ShowAsync()
    {
        Console.Write("Username: ");
        var username = Console.ReadLine() ?? "";
        Console.Write("Password: ");
        var password = Console.ReadLine() ?? "";

        var user = new User { Username = username, Password = password };
        var created = await _userRepo.AddAsync(user);

        Console.WriteLine($"User created with id {created.Id}");
        Console.ReadKey();
    }
}