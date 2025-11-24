using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RepositoryContracts;

namespace EfcRepositories;

public class EfcCommentRepository : ICommentRepository
{
    private readonly AppContext ctx;

    public EfcCommentRepository(AppContext ctx)
    {
        this.ctx = ctx;
    }

    public async Task<Comment> AddAsync(Comment comment)
    {
        EntityEntry<Comment> entityEntry = await ctx.Comments.AddAsync(comment);
        await ctx.SaveChangesAsync();
        return entityEntry.Entity;
    }

    public async Task<Comment> UpdateAsync(Comment comment)
    {
        bool exists = await ctx.Comments.AnyAsync(c => c.Id == comment.Id);
        if (!exists)
            throw new KeyNotFoundException($"Comment with id {comment.Id} not found");

        ctx.Comments.Update(comment);
        await ctx.SaveChangesAsync();
        return comment;
    }

    public async Task DeleteAsync(int id)
    {
        Comment? existing = await ctx.Comments.SingleOrDefaultAsync(c => c.Id == id);
        if (existing is null)
            throw new KeyNotFoundException($"Comment with id {id} not found");

        ctx.Comments.Remove(existing);
        await ctx.SaveChangesAsync();
    }

    public async Task<Comment?> GetByIdAsync(int id)
    {
        return await ctx.Comments
            .SingleOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Comment>> GetAllAsync()
    {
        return await ctx.Comments.ToListAsync();
    }

    public async Task<IEnumerable<Comment>> GetByPostIdAsync(int postId)
    {
        return await ctx.Comments
            .Where(c => c.PostId == postId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Comment>> GetByUserIdAsync(int userId)
    {
        return await ctx.Comments
            .Where(c => c.UserId == userId)
            .ToListAsync();
    }
}