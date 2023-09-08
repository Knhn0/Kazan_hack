using System.Linq.Expressions;
using Hack.DAL;
using Hack.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hack.Services;

public abstract class BaseCrudServiceImpl<T> : IBaseCrudService<T> where T : class
{
    private readonly DbSet<T> _data;
    private readonly ApplicationDbContext _context;

    protected BaseCrudServiceImpl(ApplicationDbContext context, DbSet<T> data)
    {
        _context = context;
        _data = data;
    }
    
    public async Task<List<T>> GetAllAsync()
    {
        return await _data.ToListAsync();
    }

    public async Task<T> FindFirstAsync(Expression<Func<T, bool>> exp)
    {
        return await _data.FirstAsync(exp);
    }

    public async Task<List<T>> FindManyAsync(Expression<Func<T, bool>> exp)
    {
        return await _data.Where(exp).ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _data.FindAsync(id);
    }

    public async Task<T> UpdateAsync(T t)
    {
        await Task.Run(() => _data.Update(t));
        await _context.SaveChangesAsync();
        return t;
    }

    public async Task<T> CreateAsync(T t)
    {
        await _data.AddAsync(t);
        return t;
    }

    public async Task RemoveAsync(T t)
    {
        await Task.Run(() => _data.Remove(t));
        await _context.SaveChangesAsync();
    }
}