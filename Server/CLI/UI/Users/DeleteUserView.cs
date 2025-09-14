using RepositoryContracts;

namespace CLI.UI.Users;

public class DeleteUserView
{
    private readonly IUserRepository _userRepo;

    public DeleteUserView(IUserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    public async Task ShowAsync()
    {
        Console.Write("User id to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) return;

        await _userRepo.DeleteAsync(id);
        Console.WriteLine("User deleted.");
        Console.ReadKey();
    }
}