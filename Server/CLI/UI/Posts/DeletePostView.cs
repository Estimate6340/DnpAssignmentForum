using RepositoryContracts;
using Entities;

namespace CLI.UI.Posts;

public class DeletePostView
{
    private readonly IPostRepository _postRepo;
    private User? _currentUser;

    public DeletePostView(IPostRepository postRepo, ref User? currentUser)
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

        Console.Write("Post id to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) return;

        var post = await _postRepo.GetByIdAsync(id);
        if (post == null || post.UserId != _currentUser.Id)
        {
            Console.WriteLine("Post not found or not owned by you.");
            Console.ReadKey();
            return;
        }

        await _postRepo.DeleteAsync(id);
        Console.WriteLine("Post deleted.");
        Console.ReadKey();
    }
}