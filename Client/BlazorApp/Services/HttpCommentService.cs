using System.Text.Json;
using System.Net.Http.Json;
using ApiContracts.DTOs;

namespace BlazorApp.Services;

public sealed class HttpCommentService : ICommentService
{
    private readonly HttpClient _http;
    private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };
    public HttpCommentService(HttpClient http) => _http = http;

    public async Task<IReadOnlyList<CommentDTO>> GetForPostAsync(int postId)
    {
        var json = await _http.GetStringAsync($"posts/{postId}/comments");
        return JsonSerializer.Deserialize<IReadOnlyList<CommentDTO>>(json, JsonOpts)!;
    }

    public async Task<CommentDTO> AddAsync(CreateCommentDTO create)
    {
        var res = await _http.PostAsJsonAsync("comments", create);
        var payload = await res.Content.ReadAsStringAsync();
        if (!res.IsSuccessStatusCode) throw new Exception(payload);
        return JsonSerializer.Deserialize<CommentDTO>(payload, JsonOpts)!;
    }
}