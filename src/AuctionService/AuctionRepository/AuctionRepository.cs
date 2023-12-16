using AuctionService.Data;
using AuctionService.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AuctionService;

public class AuctionRepository : IAuctionRepository
{
    private readonly AuctionDbContext _context;

    public AuctionRepository(AuctionDbContext context)
    {
        _context = context;

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

    public async Task<List<Auction>> GetAllAuctionsAsync()
    {
        return await _context.Auctions
            .Include(x => x.Item)
            .OrderBy(x => x.Item.Make)
            .ToListAsync();
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
