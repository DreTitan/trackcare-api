using MediatR;
using TrackCare.Application.DTOs;
using TrackCare.Domain.Interfaces;

namespace TrackCare.Application.Features.Recolhimentos.Queries;

public record GetRecentRecolhimentosQuery(int Count) : IRequest<IEnumerable<RecolhimentoDto>>;

public class GetRecentRecolhimentosQueryHandler : IRequestHandler<GetRecentRecolhimentosQuery, IEnumerable<RecolhimentoDto>>
{
    private readonly IRecolhimentoRepository _repository;
    private readonly AutoMapper.IMapper _mapper;

    public GetRecentRecolhimentosQueryHandler(IRecolhimentoRepository repository, AutoMapper.IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RecolhimentoDto>> Handle(GetRecentRecolhimentosQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetRecentAsync(request.Count);
        return _mapper.Map<IEnumerable<RecolhimentoDto>>(result);
    }
}
