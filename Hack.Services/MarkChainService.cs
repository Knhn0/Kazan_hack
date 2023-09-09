using Hack.DAL;
using Hack.Domain.Entites;
using Hack.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hack.Services;

public class MarkChainService : BaseCrudServiceImpl<MarkChain>, IMarkChainService
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<MarkChain> _data;
    
    public MarkChainService(ApplicationDbContext context, DbSet<MarkChain> data) : base(context, data)
    {
        _context = context;
        _data = data;
    }

    public async Task AppendMark(int chainId, Mark mark)
    {
        var candidate = await GetByIdAsync(chainId);
        if (candidate is null) throw new Exception("Chain not found");
        candidate.Marks.Add(mark);
        await this.UpdateAsync(candidate);
    }

    public async Task RemoveMark(int chainId, int markId)
    {
        var candidate = await GetByIdAsync(chainId);
        if (candidate is null) throw new Exception("Chain not found");
        candidate.Marks.RemoveAll(m => m.Id == markId);
        await this.UpdateAsync(candidate);
    }

    public async Task<List<Mark>?> GetMarks(int chainId)
    {
        var candidate = await GetByIdAsync(chainId);
        if (candidate is null) throw new Exception("Chain not found");
        return candidate.Marks;
    }
}