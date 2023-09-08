/*using Hack.DAL.Interfaces;
using Hack.Domain.Entites;
using Microsoft.EntityFrameworkCore;

namespace Hack.DAL.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _db;

    public UserRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<bool> CreateAsync(User user)
    {
        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();
        return true;
    }
    
    public async Task<User> GetAsync(Guid id)
    {
        var result = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
        return result ?? throw new Exception("User not found");
        
    }

    public async Task<User> UpdateAsync(User user)
    {
        var u = await _db.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
        if (u == null)
        {
            throw new Exception("User not found"); // todo
        }
        u.Email = user.Email;
        u.PasswordHashed = user.PasswordHashed;
        
        await _db.SaveChangesAsync();
        return u;
    }

    public async Task<List<User>> SelectAsync()
    {
        return await _db.Users.ToListAsync();
    }

    public async Task<bool> DeleteAsync(User entity)
    {
        _db.Users.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }
}*/