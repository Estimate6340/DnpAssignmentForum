namespace BlazorApp.Services;

using ApiContracts.DTOs;

public interface IUserService
{
    public Task<UserDTO> AddUserAsync(CreateUserDTO request);
    public Task UpdateUserAsync(int id, UserDTO request);
    Task<UserDTO?> GetUserByIdAsync(int id);
    Task<IReadOnlyList<UserDTO>> GetAllUsersAsync(string? search = null);
    Task DeleteUserAsync(int id);
    
    // ... more methods
}