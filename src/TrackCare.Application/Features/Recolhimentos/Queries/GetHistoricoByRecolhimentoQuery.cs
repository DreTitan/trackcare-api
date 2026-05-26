using MediatR;
using TrackCare.Application.DTOs;
using TrackCare.Domain.Interfaces;

namespace TrackCare.Application.Features.Recolhimentos.Queries;

public record GetHistoricoByRecolhimentoQuery(int RecolhimentoId) : IRequest<IEnumerable<HistoricoStatusDto>>;

public class GetHistoricoByRecolhimentoQueryHandler : IRequestHandler<GetHistoricoByRecolhimentoQuery, IEnumerable<HistoricoStatusDto>>
{
    private readonly IHistoricoStatusRepository _repository;
    private readonly AutoMapper.IMapper _mapper;

    public GetHistoricoByRecolhimentoQueryHandler(IHistoricoStatusRepository repository, AutoMapper.IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<HistoricoStatusDto>> Handle(GetHistoricoByRecolhimentoQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetByRecolhimentoIdAsync(request.RecolhimentoId);
        return _mapper.Map<IEnumerable<HistoricoStatusDto>>(result);
    }
}
