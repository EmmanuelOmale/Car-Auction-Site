using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace AuctionService;

public class AuctionRepository : IAuctionRepository
{
    private readonly AuctionDbContext _context;
    private readonly IMapper _mapper;

    public AuctionRepository(AuctionDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;

    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context
            .SaveChangesAsync() > 0;
    }

    public async Task<Auction> CreateNewAuctionAsync(Auction auction)
    {
        await _context.Auctions
            .AddAsync(auction);
        return auction;
        
    }

    public async Task<List<AuctionDto>> GetAllAuctionsAsync(string date)
    {
        var query = _context.Auctions.OrderBy(x => x.Item.Make).AsQueryable();

        if (!string.IsNullOrEmpty(date))
        {
            query = query.Where(x => x.UpdatedAt.CompareTo(DateTime.Parse(date).ToUniversalTime()) > 0);
        }
        // return await _context.Auctions
        //     .Include(x => x.Item)
        //     .OrderBy(x => x.Item.Make)
        //     .ToListAsync();
        return await query.ProjectTo<AuctionDto>(_mapper.ConfigurationProvider).ToListAsync();
    }

    public async Task<Auction> GetAuctionByIdAsync(Guid id)
    {
        return await _context.Auctions
            .Include(x => x.Item)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task DeleteAuctionAsync(Auction auction)
    {
        _context.Auctions.Remove(auction);
        await _context.SaveChangesAsync();

    }

    public async Task<Auction> FindAuctionAsync (Guid id)
    {
        return await _context.Auctions
            .FindAsync(id);
    }

}
