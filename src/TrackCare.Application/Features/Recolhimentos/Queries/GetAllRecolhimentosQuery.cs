using MediatR;
using TrackCare.Application.DTOs;
using TrackCare.Domain.Interfaces;

namespace TrackCare.Application.Features.Recolhimentos.Queries;

public record GetAllRecolhimentosQuery : IRequest<IEnumerable<RecolhimentoDto>>;

public class GetAllRecolhimentosQueryHandler : IRequestHandler<GetAllRecolhimentosQuery, IEnumerable<RecolhimentoDto>>
{
    private readonly IRecolhimentoRepository _repository;
    private readonly AutoMapper.IMapper _mapper;

    public GetAllRecolhimentosQueryHandler(IRecolhimentoRepository repository, AutoMapper.IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RecolhimentoDto>> Handle(GetAllRecolhimentosQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<RecolhimentoDto>>(result);
    }
}
