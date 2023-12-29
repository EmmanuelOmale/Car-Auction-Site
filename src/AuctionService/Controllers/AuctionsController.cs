using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers;

[ApiController]
[Route("api/auctions")]
public class AuctionsController : ControllerBase
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IAuctionService _auctionService;
    private readonly AuctionDbContext _context;
    private readonly IMapper _mapper;

    public AuctionsController(IAuctionService auctionService, IMapper mapper, IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
        _auctionService = auctionService;
        _mapper = mapper;
    } 

    [HttpGet]
    public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions(string date)
    {
       return await _auctionService.GetAllAuctionsAsync(date);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
    {
        return await _auctionService.GetAuctionByIdAsync(id);
    }

    [HttpPost]
    public async Task<ActionResult<AuctionDto>> CreateAuction([FromBody] CreateAuctionDto auctionDto)
    {
          var newAuctionToPublish = await _auctionService.CreateAuction(auctionDto);
          await _publishEndpoint.Publish(newAuctionToPublish);
          var saveChangesResult = await _auctionService.SaveChangesAsync();

          if (!saveChangesResult) return new BadRequestObjectResult("Failed to save changes to db");

          return Ok(newAuctionToPublish);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDto updateAuctionDto)
    {

        try 
        {
            var auction = await _context.Auctions.Include(x => x.Item)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (auction == null || auction.Item == null) return NotFound();
        // TODO: check seller == username

        auction.Item.Make = updateAuctionDto.Make ?? auction.Item.Make;
        auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model;
        auction.Item.Color = updateAuctionDto.Color ?? auction.Item.Color;
        auction.Item.Mileage = updateAuctionDto.Mileage ?? auction.Item.Mileage;
        auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;

        var result = await _context.SaveChangesAsync() > 0;

        if (result) return Ok();

        return BadRequest("Problem saving changes");

        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
       // var auctionItem = await _auctionService.GetAuctionByIdAsync(id);
       // TODO: Update endpoints not working.
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAuctionAsync(Guid id)
    {
        var result = await _auctionService.FindAuctionAsync(id);
        if (result == null) return NotFound();

        await _auctionService.DeleteAuctionAsync(result);
        return Ok();
    }
}