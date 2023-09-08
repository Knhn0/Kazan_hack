using Hack.Domain.Entites;

namespace Hack.Domain.Dto;

public class TransactionResult
{
    public EconomyTransaction Transaction { get; set; }
    public int Result { get; set; }
}