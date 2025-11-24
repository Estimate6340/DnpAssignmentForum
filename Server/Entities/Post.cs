namespace Entities;

public class Post
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    public User User { get; private set; } = null!;
    public string Title { get; set; } = "";
    public string Body { get; set; } = "";
    
    public ICollection<Comment> Comments { get; private set; } = new List<Comment>();
    private Post() {} 
    
    public Post(int userId, string title, string body)
    {
        UserId = userId;
        Title = title;
        Body = body;
    }

    public override string ToString() => $"{Id} : {Title} (user {UserId})";
}