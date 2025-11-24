using ApiContracts.DTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApplication.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository _commentRepo;
    private readonly IUserRepository _userRepo;
    private readonly IPostRepository _postRepo;

    public CommentsController(ICommentRepository commentRepo, IUserRepository userRepo, IPostRepository postRepo)
    {
        _commentRepo = commentRepo;
        _userRepo = userRepo;
        _postRepo = postRepo;
    }

    // GET /Comments?userId=...&postId=...
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CommentDTO>>> GetMany([FromQuery] int? userId = null, [FromQuery] int? postId = null)
    {
        IEnumerable<Comment> comments;

        if (postId.HasValue)
            comments = await _commentRepo.GetByPostIdAsync(postId.Value);
        else if (userId.HasValue)
            comments = await _commentRepo.GetByUserIdAsync(userId.Value);
        else
            comments = await _commentRepo.GetAllAsync();

        var dtos = new List<CommentDTO>();
        foreach (var c in comments)
        {
            dtos.Add(new CommentDTO()
            {
                Id = c.Id,
                PostId = c.PostId,
                UserId = c.UserId,
                Body = c.Body,
                Username = (await _userRepo.GetByIdAsync(c.UserId))?.Username ?? ""
            });
        }

        return Ok(dtos);
    }

    // GET /posts/{postId}/comments
    [HttpGet("/posts/{postId:int}/comments")]
    public async Task<ActionResult<IEnumerable<CommentDTO>>> GetByPost(int postId)
    {
        var comments = await _commentRepo.GetByPostIdAsync(postId);
        var dtos = comments.Select(async c => new CommentDTO()
        {
            Id = c.Id,
            PostId = c.PostId,
            UserId = c.UserId,
            Body = c.Body,
            Username = (await _userRepo.GetByIdAsync(c.UserId))?.Username ?? ""
        }).ToArray();

        // resolve tasks
        var resolved = await Task.WhenAll(dtos);
        return Ok(resolved);
    }

    // GET /Comments/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CommentDTO>> GetById(int id)
    {
        var c = await _commentRepo.GetByIdAsync(id);
        if (c == null) return NotFound();

        return Ok(new CommentDTO()
        {
            Id = c.Id,
            PostId = c.PostId,
            UserId = c.UserId,
            Body = c.Body,
            Username = (await _userRepo.GetByIdAsync(c.UserId))?.Username ?? ""
        });
    }

    // POST /Comments
    [HttpPost]
    public async Task<ActionResult<CommentDTO>> Create([FromBody] CreateCommentDTO create)
    {
        // validate post and user exist
        var post = await _postRepo.GetByIdAsync(create.PostId);
        if (post == null) return BadRequest("Post not found.");

        var user = await _userRepo.GetByIdAsync(create.UserId);
        if (user == null) return BadRequest("User not found.");

        var comment = new Comment(create.PostId, create.UserId, create.Body);
        var created = await _commentRepo.AddAsync(comment);

        var dto = new CommentDTO()
        {
            Id = created.Id,
            PostId = created.PostId,
            UserId = created.UserId,
            Body = created.Body,
            Username = user.Username
        };

        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    // PUT /Comments/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CommentDTO dto)
    {
        if (id != dto.Id) return BadRequest("Id mismatch");

        var existing = await _commentRepo.GetByIdAsync(id);
        if (existing == null) return NotFound();

        existing.Body = dto.Body;
        await _commentRepo.UpdateAsync(existing);

        return NoContent();
    }

    // DELETE /Comments/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _commentRepo.GetByIdAsync(id);
        if (existing == null) return NotFound();

        await _commentRepo.DeleteAsync(id);
        return NoContent();
    }
}