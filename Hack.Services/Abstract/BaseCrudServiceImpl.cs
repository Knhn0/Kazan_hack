using System.Linq.Expressions;
using Hack.DAL.Interfaces;
using Hack.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hack.Services.Abstract;

public abstract class BaseCrudServiceImpl<T> : IBaseCrudService<T> where T : class
{
    private readonly IBaseRepository<T> _repository;

    protected BaseCrudServiceImpl(IBaseRepository<T> repository)
    {
        _repository = repository;
    }
    
    public async Task<List<T>> GetAllAsync()
    {
        return await _repository.GetDbSet().ToListAsync();
    }

    public async Task<T> FindFirstAsync(Expression<Func<T, bool>> exp)
    {
        return await _repository.GetDbSet().FirstAsync(exp);
    }

    public async Task<List<T>> FindManyAsync(Expression<Func<T, bool>> exp)
    {
        return await _repository.GetDbSet().Where(exp).ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _repository.GetDbSet().FindAsync(id);
    }

    public async Task<T> UpdateAsync(T t)
    {
        await Task.Run(() => _repository.GetDbSet().Update(t));
        await _repository.SaveChangesAsync();
        return t;
    }

    public async Task<T> CreateAsync(T t)
    {
        await _repository.GetDbSet().AddAsync(t);
        return t;
    }

    public async Task RemoveAsync(T t)
    {
        await Task.Run(() => _repository.GetDbSet().Remove(t));
        await _repository.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _repository.SaveChangesAsync();
    }

    public Task SaveChangesAsync()
    {
        throw new NotImplementedException();
    }
}