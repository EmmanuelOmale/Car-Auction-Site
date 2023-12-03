using AuctionService.DTOs;
using AutoMapper;

namespace AuctionService.AuctionService;

public class AuctionService : IAuctionService
{
    private readonly IAuctionRepository _auctionRepository;
    private readonly IMapper _mapper;
    public AuctionService(IAuctionRepository auctionRepository, IMapper mapper)
    {
        _auctionRepository = auctionRepository;
        _mapper = mapper;
    }
    public async Task<List<AuctionDto>> GetAllAuctions()
    {
        var auctions = await _auctionRepository.GetAllAuctions();
        return _mapper.Map<List<AuctionDto>>(auctions);
    }
}
