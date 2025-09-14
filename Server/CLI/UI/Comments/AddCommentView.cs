using RepositoryContracts;
using Entities;

namespace CLI.UI.Comments;

public class AddCommentView
{
    private readonly ICommentRepository _commentRepo;
    private readonly IPostRepository _postRepo;
    private readonly IUserRepository _userRepo;
    private User? _currentUser;

    public AddCommentView(ICommentRepository commentRepo, IPostRepository postRepo, IUserRepository userRepo, ref User? currentUser)
    {
        _commentRepo = commentRepo;
        _postRepo = postRepo;
        _userRepo = userRepo;
        _currentUser = currentUser;
    }

    public async Task ShowAsync()
    {
        if (_currentUser == null)
        {
            Console.WriteLine("Login required to comment.");
            Console.ReadKey();
            return;
        }

        Console.Write("Post id: ");
        if (!int.TryParse(Console.ReadLine(), out int postId)) return;

        var post = await _postRepo.GetByIdAsync(postId);
        if (post == null)
        {
            Console.WriteLine("Post not found.");
            Console.ReadKey();
            return;
        }

        Console.Write("Comment: ");
        var body = Console.ReadLine() ?? "";

        var comment = await _commentRepo.AddAsync(new Comment
        {
            PostId = postId,
            UserId = _currentUser.Id,
            Body = body
        });

        Console.WriteLine($"Comment {comment.Id} added.");
        Console.ReadKey();
    }
}