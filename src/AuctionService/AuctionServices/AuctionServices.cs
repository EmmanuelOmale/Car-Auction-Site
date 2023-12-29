using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AuctionService;

public class AuctionServices : IAuctionService
{
    private readonly IAuctionRepository _auctionRepository;
    private readonly IMapper _mapper;
    public AuctionServices(IAuctionRepository auctionRepository, IMapper mapper)
    {
        _auctionRepository = auctionRepository;
        _mapper = mapper;
    }

    public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto auctionDto)
    {
        var newAuction = _mapper.Map<Auction>(auctionDto);
        newAuction.Seller = "Zest";
        var createdAuction = await _auctionRepository.CreateNewAuctionAsync(newAuction);
        // var saveChangesResult = await _auctionRepository.SaveChangesAsync();

        // if (!saveChangesResult)
        //     return new BadRequestObjectResult("Failed to save changes to DB");

        var auctionDtoResult = _mapper.Map<AuctionDto>(createdAuction);
        return auctionDtoResult;
        //return CreatedAtAction(nameof(GetAuctionByIdAsync))
    }

    public async Task DeleteAuctionAsync(Auction auction)
    {
        await _auctionRepository.DeleteAuctionAsync(auction);
    }

    public async Task<List<AuctionDto>> GetAllAuctionsAsync(string date)
    {
        var auctions = await _auctionRepository.GetAllAuctionsAsync(date);
        return _mapper.Map<List<AuctionDto>>(auctions);
    }

    public async Task<AuctionDto> GetAuctionByIdAsync(Guid id)
    {
        var auction = await _auctionRepository.GetAuctionByIdAsync(id);
        return _mapper.Map<AuctionDto>(auction);
    }
    public async Task<Auction> FindAuctionAsync(Guid id)
    {
        return await _auctionRepository.FindAuctionAsync(id);
    }

    public Task<bool> SaveChangesAsync()
    {
        return _auctionRepository.SaveChangesAsync();
    }
}
