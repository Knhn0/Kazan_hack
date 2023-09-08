using Hack.Domain.Dto;
using Hack.Domain.Entites;
using Hack.Services.Interfaces;

namespace Hack.Services.Economy;

public interface IEconomyService : IBaseCrudService<EconomyTransaction>
{
    Task<EconomyTransaction> BuyPromo(Guid consumerId, int promoId);
}