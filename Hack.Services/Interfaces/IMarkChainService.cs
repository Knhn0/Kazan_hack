using Hack.Domain.Entities;

namespace Hack.Services.Interfaces;

public interface IMarkChainService : IBaseCrudService<MarkChain>
{
    Task AppendMark(int chainId, Mark mark);
    Task RemoveMark(int chainId, int markId);
    Task<List<Mark>?> GetMarks(int chainId);
}