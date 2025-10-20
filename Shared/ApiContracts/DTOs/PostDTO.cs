namespace ApiContracts.DTOs;

public class PostDTO
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; } = "";
    public string Title { get; set; } = "";
    public string Body { get; set; } = "";
    public List<CommentDTO>? Comments { get; set; }
}