namespace Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";

    public override string ToString() => $"{Id}\t{Username}";
}