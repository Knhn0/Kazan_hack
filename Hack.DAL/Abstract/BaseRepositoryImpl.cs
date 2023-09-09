using System.Linq.Expressions;
using Hack.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hack.DAL.Abstract;

public abstract class BaseRepositoryImpl<T> : IBaseRepository<T> where T : class
{
    private readonly DbSet<T> _data;
    private readonly ApplicationDbContext _context;

    protected BaseRepositoryImpl(ApplicationDbContext context)
    {
        _context = context;

        var prop = context.GetType().GetProperties()
            .Where(p => p.GetType().ContainsGenericParameters)
            .First(p => p.GetType().GetGenericArguments()[0] == typeof(DbSet<T>));
        
        //var prop = context.GetType()
        //    .GetProperties()
        //    .First(c => c.GetType().GetGenericArguments().Length > 0 && c.GetType().GetGenericArguments()[0] == typeof(DbSet<T>));
        _data = prop.GetValue(_context) as DbSet<T>;
    }

    public DbSet<T> GetDbSet() => _data;
    
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

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}