using BlazorApp.Components;
using BlazorApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient<IUserService, HttpUserService>(c =>
{
    c.BaseAddress = new Uri("http://localhost:5236/");
});

builder.Services.AddHttpClient<IPostService, HttpPostService>(c =>
{
    c.BaseAddress = new Uri("http://localhost:5236/");
});
builder.Services.AddHttpClient<ICommentService, HttpCommentService>(c =>
{
    c.BaseAddress = new Uri("http://localhost:5236/");
});

builder.Services.AddScoped<ICurrentUser, CurrentUser>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();