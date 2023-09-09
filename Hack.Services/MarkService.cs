using System.Linq.Expressions;
using Hack.DAL;
using Hack.Domain.Entites;
using Hack.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hack.Services;

public class MarkService : IMarkService
{
    private readonly DbSet<Mark> _marks;
    private readonly ApplicationDbContext _context;

    public MarkService(ApplicationDbContext context)
    {
        _context = context;
        _marks = context.Marks;
    }

    public async Task<List<Mark>> GetAllAsync()
    {
        return await _marks.ToListAsync();
    }

    public async Task<Mark> FindFirstAsync(Expression<Func<Mark, bool>> exp)
    {
        return await _marks.FirstAsync(exp);
    }

    public async Task<List<Mark>> FindManyAsync(Expression<Func<Mark, bool>> exp)
    {
        return await _marks.Where(exp).ToListAsync();
    }

    public async Task<Mark?> GetByIdAsync(int id)
    {
        return await _marks.FindAsync(id);
    }

    public async Task<Mark> UpdateAsync(Mark mark)
    {
        await Task.Run(() => _marks.Update(mark));
        // мб сюда нужно сохранение бд написать
        return mark;
    }

    public async Task<Mark> CreateAsync(Mark mark)
    {
        await _marks.AddAsync(mark);
        return mark;
    }

    public async Task RemoveAsync(Mark mark)
    {
        await Task.Run(() => _marks.Remove(mark));
        // написать сохранение бд
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