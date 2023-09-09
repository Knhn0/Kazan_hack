using Hack.Domain.Entites;

namespace Hack.Services.Interfaces;

public interface IMarkChainService : IBaseCrudService<MarkChain>
{
    Task Append(int chainId, Mark mark);
    Task Remove(int chainId, Mark mark);
    Task<List<Mark>?> GetMarks(int chainId);
}