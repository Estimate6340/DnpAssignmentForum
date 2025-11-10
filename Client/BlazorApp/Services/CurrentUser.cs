using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazorApp.Services;

public interface ICurrentUser
{
    Task<int?> GetUserIdAsync();
    Task<string?> GetUserNameAsync();
}

public sealed class CurrentUser : ICurrentUser, IDisposable
{
    private readonly AuthenticationStateProvider _auth;
    private int? _userId;
    private string? _userName;

    public CurrentUser(AuthenticationStateProvider auth)
    {
        _auth = auth;
        _auth.AuthenticationStateChanged += OnAuthChanged; // keep cache in sync
    }

    public async Task<int?> GetUserIdAsync()
    {
        if (_userId.HasValue) return _userId;
        var state = await _auth.GetAuthenticationStateAsync();
        UpdateFrom(state.User);
        return _userId;
    }

    public async Task<string?> GetUserNameAsync()
    {
        if (_userName is not null) return _userName;
        var state = await _auth.GetAuthenticationStateAsync();
        UpdateFrom(state.User);
        return _userName;
    }

    private async void OnAuthChanged(Task<AuthenticationState> task)
    {
        var state = await task;
        UpdateFrom(state.User);
    }

    private void UpdateFrom(ClaimsPrincipal user)
    {
        if (user.Identity?.IsAuthenticated != true)
        {
            _userId = null;
            _userName = null;
            return;
        }

        // Prefer your custom "Id" claim; fall back to NameIdentifier if you switch later
        var idStr = user.FindFirst("Id")?.Value ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _userId = int.TryParse(idStr, out var id) ? id : null;
        _userName = user.Identity?.Name;
    }

    public void Dispose()
    {
        _auth.AuthenticationStateChanged -= OnAuthChanged;
    }
}