using Hack.Domain.Entites;

namespace Hack.DAL.Interfaces;

public interface IUserRepository : IBaseRepository<User>
{
    Task<bool> CreateAsync(User user);
    
    Task<User?> GetAsync(Guid id);

    Task<User> UpdateAsync(User user);
    
    Task<bool> DeleteAsync(User entity);
}