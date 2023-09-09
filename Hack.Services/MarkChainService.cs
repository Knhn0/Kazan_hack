using Hack.DAL;
using Hack.DAL.Interfaces;
using Hack.Domain.Entities;
using Hack.Services.Abstract;
using Hack.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hack.Services;

public class MarkChainService : BaseCrudServiceImpl<MarkChain>, IMarkChainService
{
    private readonly IMarkChainRepository _repository;
    
    public MarkChainService(IMarkChainRepository repository) : base(repository)
    {
        _repository = repository;
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