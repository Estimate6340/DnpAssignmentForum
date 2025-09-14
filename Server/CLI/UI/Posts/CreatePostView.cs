using RepositoryContracts;
using Entities;

namespace CLI.UI.Posts;

public class CreatePostView
{
    private readonly IPostRepository _postRepo;
    private User? _currentUser;

    public CreatePostView(IPostRepository postRepo, ref User? currentUser)
    {
        _postRepo = postRepo;
        _currentUser = currentUser;
    }

    public async Task ShowAsync()
    {
        if (_currentUser == null)
        {
            Console.WriteLine("You must be logged in to create a post.");
            Console.ReadKey();
            return;
        }

        Console.Write("Title: ");
        var title = Console.ReadLine() ?? "";
        Console.Write("Body: ");
        var body = Console.ReadLine() ?? "";

        var created = await _postRepo.AddAsync(new Post
        {
            Title = title,
            Body = body,
            UserId = _currentUser.Id
        });

        Console.WriteLine($"Post created with id {created.Id}.");
        Console.ReadKey();
    }
}