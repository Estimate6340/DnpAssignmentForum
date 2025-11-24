using ApiContracts.DTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApplication.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepo;

    public UsersController(IUserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    // GET /Users?search=...
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetMany([FromQuery] string? search = null)
    {
        var users = (await _userRepo.GetAllAsync()).AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            users = users.Where(u => u.Username.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        var dtos = users.Select(u => new UserDTO { Id = u.Id, Username = u.Username }).ToList();
        return Ok(dtos);
    }

    // GET /Users/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDTO>> GetById(int id)
    {
        var user = await _userRepo.GetByIdAsync(id);
        if (user == null) return NotFound();
        return Ok(new UserDTO { Id = user.Id, Username = user.Username });
    }

    // POST /Users
    [HttpPost]
    public async Task<ActionResult<UserDTO>> Create([FromBody] CreateUserDTO create)
    {
        // check if username already exists
        var existing = await _userRepo.GetByUsernameAsync(create.Username);
        if (existing != null)
            return BadRequest($"Username '{create.Username}' already exists.");

        var user = new User(create.Username, create.Password);
        var created = await _userRepo.AddAsync(user);


        var dto = new UserDTO { Id = created.Id, Username = created.Username };
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    // PUT /Users/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UserDTO dto)
    {
        if (id != dto.Id) return BadRequest("Id mismatch.");

        var existing = await _userRepo.GetByIdAsync(id);
        if (existing == null) return NotFound();

        existing.Username = dto.Username;
        await _userRepo.UpdateAsync(existing);

        return NoContent();
    }

    // DELETE /Users/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _userRepo.GetByIdAsync(id);
        if (existing == null) return NotFound();

        await _userRepo.DeleteAsync(id);
        return NoContent();
    }
}