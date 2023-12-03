using AuctionService.Entities;

namespace AuctionService;

public interface IAuctionRepository
{
    Task<List<Auction>> GetAllAuctions();

}
