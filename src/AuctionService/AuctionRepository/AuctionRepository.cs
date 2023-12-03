using AuctionService.Data;
using AuctionService.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Repository;

public class AuctionRepository : IAuctionRepository
{
    private readonly AuctionDbContext _context;
    private readonly IMapper _mapper;
    public AuctionRepository(AuctionDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;

    }
    public async Task<List<Auction>> GetAllAuctions()
    {
        return await _context.Auctions
            .Include(x => x.Item)
            .OrderBy(x => x.Item.Make)
            .ToListAsync();
    }
}
