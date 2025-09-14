using RepositoryContracts;

namespace CLI.UI.Posts;

public class ListPostsView
{
    private readonly IPostRepository _postRepo;

    public ListPostsView(IPostRepository postRepo)
    {
        _postRepo = postRepo;
    }

    public async Task ShowAsync()
    {
        var posts = await _postRepo.GetAllAsync();
        Console.WriteLine("=== POSTS ===");
        foreach (var post in posts)
        {
            Console.WriteLine($"{post.Id} : {post.Title}");
        }
        Console.ReadKey();
    }
}