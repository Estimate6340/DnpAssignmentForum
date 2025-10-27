namespace ApiContracts.DTOs;

public class CreatePostDTO
{
    public int UserId { get; set; }        // author id
    public required string Title { get; set; }
    public required string Body { get; set; }
}