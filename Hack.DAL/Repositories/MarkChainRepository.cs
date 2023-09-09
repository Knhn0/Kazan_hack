using System.Linq.Expressions;
using Hack.DAL.Interfaces;
using Hack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hack.DAL.Repositories;

public class MarkChainRepository : IMarkChainRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<MarkChain> _data;

    public MarkChainRepository(ApplicationDbContext context)
    {
        _context = context;
        _data = context.MarkChains;
    }
    
    public DbSet<MarkChain> GetDbSet() => _data;
    
    public async Task<List<MarkChain>> GetAllAsync()
    {
        return await _data.ToListAsync();
    }

    public async Task<MarkChain> FindFirstAsync(Expression<Func<MarkChain, bool>> exp)
    {
        return await _data.FirstAsync(exp);
    }

    public async Task<List<MarkChain>> FindManyAsync(Expression<Func<MarkChain, bool>> exp)
    {
        return await _data.Where(exp).ToListAsync();
    }

    public async Task<MarkChain?> GetByIdAsync(int id)
    {
        return await _data.FindAsync(id);
    }

    public async Task<MarkChain> UpdateAsync(MarkChain t)
    {
        await Task.Run(() => _data.Update(t));
        await _context.SaveChangesAsync();
        return t;
    }

    public async Task<MarkChain> CreateAsync(MarkChain t)
    {
        await _data.AddAsync(t);
        return t;
    }

    public async Task RemoveAsync(MarkChain t)
    {
        await Task.Run(() => _data.Remove(t));
        await _context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}