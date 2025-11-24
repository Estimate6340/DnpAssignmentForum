namespace Entities;

public class Comment
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public int UserId { get; set; }
    
    public Post Post { get; private set; } = null!;
    public User User { get; private set; } = null!;
    public string Body { get; set; } = "";
    private Comment() {}
    
    public Comment(int postId, int userId, string body)
    {
        PostId = postId;
        UserId = userId;
        Body = body;
    }
    public override string ToString() => $"{Id} (post {PostId}, user {UserId}): {Body}";
}