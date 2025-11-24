using EfcRepositories;
using RepositoryContracts;
using Microsoft.EntityFrameworkCore;

var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

// -----------------------------------------
// Services
// -----------------------------------------
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
builder.Services.AddDbContext<EfcRepositories.AppContext>();

// EF Repositories
builder.Services.AddScoped<IPostRepository, EfcPostRepository>();
builder.Services.AddScoped<IUserRepository, EfcUserRepository>();
builder.Services.AddScoped<ICommentRepository, EfcCommentRepository>();

var app = builder.Build();

// -----------------------------------------
// Middleware pipeline
// -----------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();