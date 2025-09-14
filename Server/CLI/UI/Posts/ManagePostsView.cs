using RepositoryContracts;
using Entities;

namespace CLI.UI.Posts;

public class ManagePostsView
{
    private readonly IPostRepository _postRepo;
    private readonly IUserRepository _userRepo;
    private User? _currentUser;

    public ManagePostsView(IPostRepository postRepo, IUserRepository userRepo, ref User? currentUser)
    {
        _postRepo = postRepo;
        _userRepo = userRepo;
        _currentUser = currentUser;
    }

    public async Task ShowAsync()
    {
        Console.Clear();
        Console.WriteLine("=== POSTS MENU ===");
        Console.WriteLine("1) Create post");
        Console.WriteLine("2) List posts");
        Console.WriteLine("3) View single post");
        Console.WriteLine("4) Update post");
        Console.WriteLine("5) Delete post");
        Console.WriteLine("0) Back");
        Console.Write("Choice: ");
        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                var create = new CreatePostView(_postRepo, ref _currentUser);
                await create.ShowAsync();
                break;
            case "2":
                var list = new ListPostsView(_postRepo);
                await list.ShowAsync();
                break;
            case "3":
                var single = new SinglePostView(_postRepo, _userRepo);
                await single.ShowAsync();
                break;
            case "4":
                var update = new UpdatePostView(_postRepo, ref _currentUser);
                await update.ShowAsync();
                break;
            case "5":
                var delete = new DeletePostView(_postRepo, ref _currentUser);
                await delete.ShowAsync();
                break;
        }
    }
}