using System.Linq.Expressions;
using Hack.DAL.Abstract;
using Hack.DAL.Interfaces;
using Hack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hack.DAL.Repositories;

public class MarkChainRepository : BaseRepositoryImpl<MarkChain>, IMarkChainRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<MarkChain> _data;

    public MarkChainRepository(ApplicationDbContext context, DbSet<MarkChain> data) : base(context, data)
    {
        _context = context;
        _data = data;
    }
}