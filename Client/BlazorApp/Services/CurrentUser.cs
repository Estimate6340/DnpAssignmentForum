namespace BlazorApp.Services;

public interface ICurrentUser { int UserId { get; } }
public sealed class CurrentUser : ICurrentUser
{
    //FIXED USER -> NOT FIXED LATER
    public int UserId => 1;
}