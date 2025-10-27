using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using ApiContracts.DTOs;

namespace BlazorApp.Services;

public sealed class HttpUserService : IUserService
{
    private readonly HttpClient _http;
    private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };

    public HttpUserService(HttpClient http) => _http = http;

    public async Task<UserDTO> AddUserAsync(CreateUserDTO request)
    {
        var res = await _http.PostAsJsonAsync("users", request);
        var payload = await res.Content.ReadAsStringAsync();
        if (!res.IsSuccessStatusCode) throw new Exception(payload);
        return JsonSerializer.Deserialize<UserDTO>(payload, JsonOpts)!;
    }

    public async Task UpdateUserAsync(int id, UserDTO request)
    {
        var res = await _http.PutAsJsonAsync($"users/{id}", request);
        if (res.IsSuccessStatusCode) return;
        throw new Exception(await res.Content.ReadAsStringAsync());
    }

    public async Task<UserDTO?> GetUserByIdAsync(int id)
    {
        var res = await _http.GetAsync($"users/{id}");
        if (res.StatusCode == HttpStatusCode.NotFound) return null;
        var payload = await res.Content.ReadAsStringAsync();
        if (!res.IsSuccessStatusCode) throw new Exception(payload);
        return JsonSerializer.Deserialize<UserDTO>(payload, JsonOpts);
    }

    public async Task<IReadOnlyList<UserDTO>> GetAllUsersAsync(string? search = null)
    {
        var path = "users";
        if (!string.IsNullOrWhiteSpace(search))
            path += $"?search={Uri.EscapeDataString(search)}";

        var json = await _http.GetStringAsync(path);
        return JsonSerializer.Deserialize<IReadOnlyList<UserDTO>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
    }

    public async Task DeleteUserAsync(int id)
    {
        var res = await _http.DeleteAsync($"users/{id}");
        if (res.IsSuccessStatusCode) return;
        throw new Exception(await res.Content.ReadAsStringAsync());
    }
}