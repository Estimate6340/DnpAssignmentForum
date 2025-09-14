using RepositoryContracts;
using Entities;

namespace CLI.UI.Users;

public class UpdateUserView
{
    private readonly IUserRepository _userRepo;

    public UpdateUserView(IUserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    public async Task ShowAsync()
    {
        Console.Write("User id to update: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) return;

        var user = await _userRepo.GetByIdAsync(id);
        if (user == null)
        {
            Console.WriteLine("User not found.");
            Console.ReadKey();
            return;
        }

        Console.Write("New username (leave empty to keep): ");
        var newName = Console.ReadLine();
        Console.Write("New password (leave empty to keep): ");
        var newPass = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(newName)) user.Username = newName;
        if (!string.IsNullOrWhiteSpace(newPass)) user.Password = newPass;

        await _userRepo.UpdateAsync(user);
        Console.WriteLine("User updated.");
        Console.ReadKey();
    }
}