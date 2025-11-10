using ApiContracts.DTOs;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using Entities;

namespace WebApplication.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public AuthController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDTO>> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest("Username and password are required.");

        var user = await _userRepository.GetByUsernameAsync(request.Username);
        if (user is null)
            return Unauthorized("User not found.");
        
        if (user.Password != request.Password)
            return Unauthorized("Incorrect password.");

        var dto = new UserDTO
        {
            Id = user.Id,
            Username = user.Username
        };

        return Ok(dto);
    }
}