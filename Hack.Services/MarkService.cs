using System.Linq.Expressions;
using Hack.DAL;
using Hack.Domain.Entites;
using Hack.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hack.Services;

public class MarkService : BaseCrudServiceImpl<Mark>, IMarkService
{
    private readonly DbSet<Mark> _data;
    private readonly ApplicationDbContext _context;

    public MarkService(ApplicationDbContext context, DbSet<Mark> data) : base(context, data)
    {
        _context = context;
        _data = data;
    }
}