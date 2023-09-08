using Hack.DAL;
using Hack.Domain.Entites;
using Hack.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hack.Services;

public class PromoService : BaseCrudServiceImpl<Promo>, IPromoService
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<Promo> _data;
    
    public PromoService(ApplicationDbContext context, DbSet<Promo> data) : base(context, data)
    {
        _context = context;
        _data = data;
    }
}