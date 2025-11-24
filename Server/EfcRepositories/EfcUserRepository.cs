using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RepositoryContracts;

namespace EfcRepositories;

public class EfcUserRepository : IUserRepository
{
    private readonly AppContext ctx;

    public EfcUserRepository(AppContext ctx)
    {
        this.ctx = ctx;
    }

    public async Task<User> AddAsync(User user)
    {
        EntityEntry<User> entityEntry = await ctx.Users.AddAsync(user);
        await ctx.SaveChangesAsync();
        return entityEntry.Entity;
    }

    public async Task<User> UpdateAsync(User user)
    {
        bool exists = await ctx.Users.AnyAsync(u => u.Id == user.Id);
        if (!exists)
            throw new KeyNotFoundException($"User with id {user.Id} not found");

        ctx.Users.Update(user);
        await ctx.SaveChangesAsync();
        return user;
    }

    public async Task DeleteAsync(int id)
    {
        User? existing = await ctx.Users.SingleOrDefaultAsync(u => u.Id == id);
        if (existing is null)
            throw new KeyNotFoundException($"User with id {id} not found");

        ctx.Users.Remove(existing);
        await ctx.SaveChangesAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await ctx.Users
            .SingleOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await ctx.Users
            .SingleOrDefaultAsync(u => u.Username == username);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await ctx.Users.ToListAsync();
    }
}