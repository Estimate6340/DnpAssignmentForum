namespace ApiContracts.DTOs;

public class CommentDTO
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; } = "";
    public string Body { get; set; } = "";
}