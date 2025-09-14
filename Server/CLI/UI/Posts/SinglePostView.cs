using RepositoryContracts;

namespace CLI.UI.Posts;

public class SinglePostView
{
    private readonly IPostRepository _postRepo;
    private readonly IUserRepository _userRepo;

    public SinglePostView(IPostRepository postRepo, IUserRepository userRepo)
    {
        _postRepo = postRepo;
        _userRepo = userRepo;
    }

    public async Task ShowAsync()
    {
        Console.Write("Post id: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) return;

        var post = await _postRepo.GetByIdAsync(id);
        if (post == null)
        {
            Console.WriteLine("Post not found.");
            Console.ReadKey();
            return;
        }

        var author = await _userRepo.GetByIdAsync(post.UserId);
        Console.WriteLine($"=== {post.Title} ===");
        Console.WriteLine($"By: {author?.Username ?? "Unknown"}");
        Console.WriteLine(post.Body);
        Console.ReadKey();
    }
}