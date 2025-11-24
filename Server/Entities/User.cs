namespace Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";

    public ICollection<Post> Posts { get; private set; } = new List<Post>();
    public ICollection<Comment> Comments { get; private set; } = new List<Comment>();
    
    private User() {}
    
    public User(string username, string password)
    {
        Username = username;
        Password = password;
    }
    public override string ToString() => $"{Id}\t{Username}";
}