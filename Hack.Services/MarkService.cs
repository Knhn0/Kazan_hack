using System.Linq.Expressions;
using Hack.DAL;
using Hack.DAL.Interfaces;
using Hack.Domain.Entities;
using Hack.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hack.Services;

public class MarkService : IMarkService
{
    private readonly IMarkRepository _repository;

    public MarkService(IMarkRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Mark>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Mark> FindFirstAsync(Expression<Func<Mark, bool>> exp)
    {
        return await _repository.FindFirstAsync(exp);
    }

    public async Task<List<Mark>> FindManyAsync(Expression<Func<Mark, bool>> exp)
    {
        return await _repository.FindManyAsync(exp);
    }

    public async Task<Mark?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Mark> UpdateAsync(Mark mark)
    {
        return await _repository.UpdateAsync(mark);
    }

    public async Task<Mark> CreateAsync(Mark mark)
    {
        return await _repository.CreateAsync(mark);
    }

    public async Task RemoveAsync(Mark mark)
    {
        await _repository.RemoveAsync(mark);
    }

    /*public async Task<Mark> GetMarkAsync(int id)
    {
        if (id.ToString().IsNullOrEmpty())
        {
            throw new Exception("id is nullable");
        }
        var responce = await _data.FindAsync(id);
        return responce;
    }*/

}