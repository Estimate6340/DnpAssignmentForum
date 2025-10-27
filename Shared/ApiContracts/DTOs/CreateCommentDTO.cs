namespace ApiContracts.DTOs;

public class CreateCommentDTO
{
    public int PostId { get; set; }
    public int UserId { get; set; }
    public required string Body { get; set; }
}