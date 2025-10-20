using ApiContracts.DTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApplication.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository _postRepo;
    private readonly IUserRepository _userRepo;
    private readonly ICommentRepository _commentRepo;

    public PostsController(IPostRepository postRepo, IUserRepository userRepo, ICommentRepository commentRepo)
    {
        _postRepo = postRepo;
        _userRepo = userRepo;
        _commentRepo = commentRepo;
    }

    // GET /Posts?title=...&userId=...&username=...&includeComments=true
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PostDTO>>> GetMany(
        [FromQuery] string? title = null,
        [FromQuery] int? userId = null,
        [FromQuery] string? username = null,
        [FromQuery] bool includeComments = false)
    {
        IEnumerable<Post> posts = await _postRepo.GetAllAsync();

        if (userId.HasValue)
        {
            // repo has GetByUserIdAsync
            posts = await _postRepo.GetByUserIdAsync(userId.Value);
        }
        else if (!string.IsNullOrWhiteSpace(username))
        {
            // find user(s) by username substring
            var users = (await _userRepo.GetAllAsync())
                .Where(u => u.Username.Contains(username, StringComparison.OrdinalIgnoreCase))
                .Select(u => u.Id)
                .ToHashSet();
            posts = posts.Where(p => users.Contains(p.UserId));
        }

        if (!string.IsNullOrWhiteSpace(title))
        {
            posts = posts.Where(p => p.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
        }

        var result = new List<PostDTO>();
        foreach (var p in posts)
        {
            var dto = new PostDTO
            {
                Id = p.Id,
                UserId = p.UserId,
                Title = p.Title,
                Body = p.Body,
                Username = (await _userRepo.GetByIdAsync(p.UserId))?.Username ?? ""
            };

            if (includeComments)
            {
                var comments = await _commentRepo.GetByPostIdAsync(p.Id);
                dto.Comments = new List<CommentDTO>();

                foreach (var c in comments)
                {
                    var commentUser = await _userRepo.GetByIdAsync(c.UserId);
                    dto.Comments.Add(new CommentDTO
                    {
                        Id = c.Id,
                        PostId = c.PostId,
                        UserId = c.UserId,
                        Body = c.Body,
                        Username = commentUser?.Username ?? ""
                    });
                }
            }

            result.Add(dto);
        }

        return Ok(result);
    }

    // GET /Posts/{id}?includeComments=true
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PostDTO>> GetById(int id, [FromQuery] bool includeComments = false)
    {
        var p = await _postRepo.GetByIdAsync(id);
        if (p == null) return NotFound();

        var dto = new PostDTO()
        {
            Id = p.Id,
            UserId = p.UserId,
            Title = p.Title,
            Body = p.Body,
            Username = (await _userRepo.GetByIdAsync(p.UserId))?.Username ?? ""
        };

        if (includeComments)
        {
            var comments = await _commentRepo.GetByPostIdAsync(p.Id);
            dto.Comments = new List<CommentDTO>();

            foreach (var c in comments)
            {
                var commentUser = await _userRepo.GetByIdAsync(c.UserId);
                dto.Comments.Add(new CommentDTO
                {
                    Id = c.Id,
                    PostId = c.PostId,
                    UserId = c.UserId,
                    Body = c.Body,
                    Username = commentUser?.Username ?? ""
                });
            }
        }

        return Ok(dto);
    }

    // POST /Posts
    [HttpPost]
    public async Task<ActionResult<PostDTO>> Create([FromBody] CreatePostDTO create)
    {
        // validate user exists
        var author = await _userRepo.GetByIdAsync(create.UserId);
        if (author == null) return BadRequest("User (author) not found.");

        var post = new Post { UserId = create.UserId, Title = create.Title, Body = create.Body };
        var created = await _postRepo.AddAsync(post);

        var dto = new PostDTO()
        {
            Id = created.Id,
            UserId = created.UserId,
            Title = created.Title,
            Body = created.Body,
            Username = author.Username
        };

        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    // PUT /Posts/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] PostDTO dto)
    {
        if (id != dto.Id) return BadRequest("Id mismatch");

        var existing = await _postRepo.GetByIdAsync(id);
        if (existing == null) return NotFound();

        existing.Title = dto.Title;
        existing.Body = dto.Body;
        await _postRepo.UpdateAsync(existing);

        return NoContent();
    }

    // DELETE /Posts/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _postRepo.GetByIdAsync(id);
        if (existing == null) return NotFound();

        await _postRepo.DeleteAsync(id);
        return NoContent();
    }
}
