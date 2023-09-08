using System.Linq.Expressions;
using Hack.DAL;
using Hack.Domain.Dto;
using Hack.Domain.Entites;
using Hack.Services.Economy;
using Hack.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hack.Services;

[Obsolete("Deprecated")]
public class EconomyService : BaseCrudServiceImpl<EconomyTransaction>, IEconomyService
{
    private readonly ILogger<EconomyService> _logger;
    private readonly IUserService _userService;
    private readonly DbSet<EconomyTransaction> _transactions;
    private readonly IPromoService _promoService;

    public EconomyService(ILogger<EconomyService> logger, IUserService userService,
        DbSet<EconomyTransaction> transactions, ApplicationDbContext context, IPromoService promoService) : base(context: context, transactions)
    {
        _logger = logger;
        _userService = userService;
        _transactions = transactions;
        _promoService = promoService;
    }
    
    public async Task<EconomyTransaction> BuyPromo(Guid consumerId, int promoId)
    {
        var consumer = await _userService.GetUserManager().FindByIdAsync(consumerId.ToString());
        if (consumer is null)
        {
            throw new Exception("User not found");
        }

        var promo = await _promoService.GetByIdAsync(promoId);

        if (promo.Price > consumer.Points)
        {
            throw new Exception("Not enough points");
        }

        var transaction = new EconomyTransaction
        {
            Price = promo.Price,
            PointsLeft = consumer.Points - promo.Price,
            PromoId = promoId,
            UserId = consumerId
        };

        return transaction;
    }
}