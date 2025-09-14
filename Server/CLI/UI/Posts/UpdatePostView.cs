using RepositoryContracts;
using Entities;

namespace CLI.UI.Posts;

public class UpdatePostView
{
    private readonly IPostRepository _postRepo;
    private User? _currentUser;

    public UpdatePostView(IPostRepository postRepo, ref User? currentUser)
    {
        _postRepo = postRepo;
        _currentUser = currentUser;
    }

    public async Task ShowAsync()
    {
        if (_currentUser == null)
        {
            Console.WriteLine("Login required.");
            Console.ReadKey();
            return;
        }

        Console.Write("Post id: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) return;

        var post = await _postRepo.GetByIdAsync(id);
        if (post == null || post.UserId != _currentUser.Id)
        {
            Console.WriteLine("Post not found or not owned by you.");
            Console.ReadKey();
            return;
        }

        Console.Write("New title (leave empty to keep): ");
        var newTitle = Console.ReadLine();
        Console.Write("New body (leave empty to keep): ");
        var newBody = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(newTitle)) post.Title = newTitle;
        if (!string.IsNullOrWhiteSpace(newBody)) post.Body = newBody;

        await _postRepo.UpdateAsync(post);
        Console.WriteLine("Post updated.");
        Console.ReadKey();
    }
}