using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using ApiContracts.DTOs;

namespace BlazorApp.Services;

public sealed class HttpPostService : IPostService
{
    private readonly HttpClient _http;
    private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };
    public HttpPostService(HttpClient http) => _http = http;

    public async Task<PostDTO> AddAsync(CreatePostDTO create)
    {
        var res = await _http.PostAsJsonAsync("posts", create);
        var payload = await res.Content.ReadAsStringAsync();
        if (!res.IsSuccessStatusCode) throw new Exception(payload);
        return JsonSerializer.Deserialize<PostDTO>(payload, JsonOpts)!;
    }

    public async Task<IReadOnlyList<PostDTO>> GetManyAsync(string? title = null, int? userId = null, string? username = null, bool includeComments = false)
    {
        var sb = new StringBuilder("posts?");
        if (!string.IsNullOrWhiteSpace(title)) sb.Append($"title={Uri.EscapeDataString(title)}&");
        if (userId.HasValue) sb.Append($"userId={userId.Value}&");
        if (!string.IsNullOrWhiteSpace(username)) sb.Append($"username={Uri.EscapeDataString(username)}&");
        if (includeComments) sb.Append("includeComments=true&");
        var path = sb.ToString().TrimEnd('&', '?');

        var json = await _http.GetStringAsync(path);
        return JsonSerializer.Deserialize<IReadOnlyList<PostDTO>>(json, JsonOpts)!;
    }

    public async Task<PostDTO?> GetByIdAsync(int id, bool includeComments = false)
    {
        var path = includeComments ? $"posts/{id}?includeComments=true" : $"posts/{id}";
        var res = await _http.GetAsync(path);
        if (res.StatusCode == HttpStatusCode.NotFound) return null;
        var payload = await res.Content.ReadAsStringAsync();
        if (!res.IsSuccessStatusCode) throw new Exception(payload);
        return JsonSerializer.Deserialize<PostDTO>(payload, JsonOpts);
    }
}
