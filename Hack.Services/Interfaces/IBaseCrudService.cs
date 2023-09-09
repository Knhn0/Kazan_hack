using System.Linq.Expressions;

namespace Hack.Services.Interfaces;

public interface IBaseCrudService<T> where T : class
{
    Task<List<T>> GetAllAsync();
    Task<T> FindFirstAsync(Expression<Func<T, bool>> exp);
    Task<List<T>> FindManyAsync(Expression<Func<T, bool>> exp);
    Task<T?> GetByIdAsync(int id);
    Task<T> UpdateAsync(T t);
    Task<T> CreateAsync(T t);
    Task RemoveAsync(T t);
    Task SaveChangesAsync();
}