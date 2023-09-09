using System.Linq.Expressions;
using Hack.DAL.Interfaces;
using Hack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hack.DAL.Repositories;

public class MarkRepository : IMarkRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<Mark> _data;

    public MarkRepository(ApplicationDbContext context)
    {
        _context = context;
        _data = context.Marks;
    }
    
    public DbSet<Mark> GetDbSet() => _data;
    
    public async Task<List<Mark>> GetAllAsync()
    {
        return await _data.ToListAsync();
    }

    public async Task<Mark> FindFirstAsync(Expression<Func<Mark, bool>> exp)
    {
        return await _data.FirstAsync(exp);
    }

    public async Task<List<Mark>> FindManyAsync(Expression<Func<Mark, bool>> exp)
    {
        return await _data.Where(exp).ToListAsync();
    }

    public async Task<Mark?> GetByIdAsync(int id)
    {
        return await _data.FindAsync(id);
    }

    public async Task<Mark> UpdateAsync(Mark t)
    {
        await Task.Run(() => _data.Update(t));
        await _context.SaveChangesAsync();
        return t;
    }

    public async Task<Mark> CreateAsync(Mark t)
    {
        await _data.AddAsync(t);
        return t;
    }

    public async Task RemoveAsync(Mark t)
    {
        await Task.Run(() => _data.Remove(t));
        await _context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}