using AuctionService.DTOs;
using AuctionService.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AuctionService;

public interface IAuctionService
{
    Task<List<AuctionDto>> GetAllAuctionsAsync();
    Task<AuctionDto> GetAuctionByIdAsync(Guid id);
    Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto auctionDto);
    Task DeleteAuctionAsync(Auction auction);
    Task<Auction> FindAuctionAsync(Guid id);
}
