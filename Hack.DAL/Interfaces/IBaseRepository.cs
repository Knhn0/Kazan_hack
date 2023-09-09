using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Hack.DAL.Interfaces;

public interface IBaseRepository<T> where T : class
{
    DbSet<T> GetDbSet();
    Task<List<T>> GetAllAsync();
    Task<T> FindFirstAsync(Expression<Func<T, bool>> exp);
    Task<List<T>> FindManyAsync(Expression<Func<T, bool>> exp);
    Task<T?> GetByIdAsync(int id);
    Task<T> UpdateAsync(T t);
    Task<T> CreateAsync(T t);
    Task RemoveAsync(T t);
    Task SaveChangesAsync();
}
