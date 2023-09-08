using System.Linq.Expressions;
using Hack.Domain.Entites;

namespace Hack.Services.Interfaces;

public interface IMarkService : IBaseCrudService<Mark>
{
    /*Task<List<Mark>> GetAllAsync();
    Task<Mark> FindFirstAsync(Expression<Func<Mark, bool>> exp);
    Task<List<Mark>> FindManyAsync(Expression<Func<Mark, bool>> exp);
    Task<Mark?> GetByIdAsync(int id);
    Task<Mark> UpdateAsync(Mark mark);
    Task<Mark> CreateAsync(Mark mark);
    Task RemoveAsync(Mark mark);*/
}