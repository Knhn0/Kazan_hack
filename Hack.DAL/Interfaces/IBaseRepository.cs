namespace Hack.DAL.Interfaces;

public interface IBaseRepository<T>
{
    Task<bool> CreateAsync(T entity);

    Task<T> GetAsync(Guid id);

    Task<List<T>> SelectAsync();
    
    Task<bool> DeleteAsync(T entity);
}
