using Microsoft.EntityFrameworkCore;

namespace Hack.Domain.Entites;

[PrimaryKey("Id")]
public class MarkChain
{
    public int Id { get; set; }
    public List<Mark> Marks { get; set; }
    
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}