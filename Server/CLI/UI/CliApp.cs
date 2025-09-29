using RepositoryContracts;
using Entities;
using CLI.UI.Users;
using CLI.UI.Posts;
using CLI.UI.Comments;

namespace CLI.UI;

public class CliApp
{
    private readonly IUserRepository _userRepo;
    private readonly IPostRepository _postRepo;
    private readonly ICommentRepository _commentRepo;

    private User? _currentUser;

    public CliApp(IUserRepository userRepo, IPostRepository postRepo, ICommentRepository commentRepo)
    {
        _userRepo = userRepo;
        _postRepo = postRepo;
        _commentRepo = commentRepo;
    }

    public async Task StartAsync()
    {
        bool running = true;
        while (running)
        {
            Console.Clear();
            Console.WriteLine("=== SIMPLE BLOG CLI ===");
            Console.WriteLine($"Logged in: {(_currentUser?.Username ?? "No")}");
            Console.WriteLine("1) Login");
            Console.WriteLine("2) Logout");
            Console.WriteLine("3) Manage users");
            Console.WriteLine("4) Manage posts");
            Console.WriteLine("5) Add comment");
            Console.WriteLine("0) Exit");
            Console.Write("Choice: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    var login = new LoginView(_userRepo);
                    _currentUser = await login.ShowAsync();
                    break;
                case "2":
                    _currentUser = null;
                    Console.WriteLine("Logged out.");
                    Console.ReadKey();
                    break;
                case "3":
                    var userMenu = new ManageUsersView(_userRepo);
                    await userMenu.ShowAsync();
                    break;
                case "4":
                    var postMenu = new ManagePostsView(_postRepo, _userRepo, ref _currentUser);
                    await postMenu.ShowAsync();
                    break;
                case "5":
                    var commentView = new AddCommentView(_commentRepo, _postRepo, _userRepo, ref _currentUser);
                    await commentView.ShowAsync();
                    break;
                case "0":
                    running = false;
                    break;
            }
        }
    }
}
