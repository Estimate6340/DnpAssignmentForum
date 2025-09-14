using RepositoryContracts;

namespace CLI.UI.Users;

public class ListUsersView
{
    private readonly IUserRepository _userRepo;

    public ListUsersView(IUserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    public async Task ShowAsync()
    {
        var users = await _userRepo.GetAllAsync();
        Console.WriteLine("=== USERS ===");
        foreach (var user in users)
        {
            Console.WriteLine(user);
        }
        Console.ReadKey();
    }
}