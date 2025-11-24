using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RepositoryContracts;

namespace EfcRepositories;

public class EfcPostRepository : IPostRepository
{
    private readonly AppContext ctx;

    public EfcPostRepository(AppContext ctx)
    {
        this.ctx = ctx;
    }

    public async Task<Post> AddAsync(Post post)
    {
        EntityEntry<Post> entityEntry = await ctx.Posts.AddAsync(post);
        await ctx.SaveChangesAsync();
        return entityEntry.Entity;
    }

    public async Task<Post> UpdateAsync(Post post)
    {
        bool exists = await ctx.Posts.AnyAsync(p => p.Id == post.Id);
        if (!exists)
            throw new KeyNotFoundException($"Post with id {post.Id} not found");

        ctx.Posts.Update(post);
        await ctx.SaveChangesAsync();
        return post;
    }

    public async Task DeleteAsync(int id)
    {
        Post? existing = await ctx.Posts.SingleOrDefaultAsync(p => p.Id == id);
        if (existing is null)
            throw new KeyNotFoundException($"Post with id {id} not found");

        ctx.Posts.Remove(existing);
        await ctx.SaveChangesAsync();
    }

    public async Task<Post?> GetByIdAsync(int id)
    {
        return await ctx.Posts
            .SingleOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Post>> GetAllAsync()
    {
        return await ctx.Posts.ToListAsync();
    }

    public async Task<IEnumerable<Post>> GetByUserIdAsync(int userId)
    {
        return await ctx.Posts
            .Where(p => p.UserId == userId)
            .ToListAsync();
    }
}