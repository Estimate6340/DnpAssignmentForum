using RepositoryContracts;

namespace CLI.UI.Users;

public class ManageUsersView
{
    private readonly IUserRepository _userRepo;

    public ManageUsersView(IUserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    public async Task ShowAsync()
    {
        Console.Clear();
        Console.WriteLine("=== USERS MENU ===");
        Console.WriteLine("1) Create user");
        Console.WriteLine("2) List users");
        Console.WriteLine("3) Update user");
        Console.WriteLine("4) Delete user");
        Console.WriteLine("0) Back");
        Console.Write("Choice: ");
        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                var create = new CreateUserView(_userRepo);
                await create.ShowAsync();
                break;
            case "2":
                var list = new ListUsersView(_userRepo);
                await list.ShowAsync();
                break;
            case "3":
                var update = new UpdateUserView(_userRepo);
                await update.ShowAsync();
                break;
            case "4":
                var delete = new DeleteUserView(_userRepo);
                await delete.ShowAsync();
                break;
        }
    }
}