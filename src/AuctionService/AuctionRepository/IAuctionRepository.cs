﻿using AuctionService.DTOs;
using AuctionService.Entities;

namespace AuctionService;

public interface IAuctionRepository
{
    Task<List<Auction>> GetAllAuctionsAsync();
    Task<Auction> GetAuctionByIdAsync(Guid id);
    Task<bool> SaveChangesAsync();
    Task<Auction> CreateNewAuctionAsync(Auction auction);
    Task DeleteAuctionAsync(Auction auction);
    Task<Auction> FindAuctionAsync (Guid id);

}
