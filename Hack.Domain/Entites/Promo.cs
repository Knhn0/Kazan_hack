using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Hack.Domain.Entities;

[PrimaryKey("Id")]
public class Promo
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int CompanyId { get; set; }
    public string CompanyHook { get; set; }
    public int Price { get; set; }
}