using Microsoft.EntityFrameworkCore;

namespace Hack.Domain.Entites;

[PrimaryKey("Id")]
public class EconomyTransaction
{
    public Guid Id { get; set; }
    
    public int Price { get; set; }
    public int PointsLeft { get; set; }
    
    public int PromoId { get; set; }
    public Guid UserId { get; set; }
}