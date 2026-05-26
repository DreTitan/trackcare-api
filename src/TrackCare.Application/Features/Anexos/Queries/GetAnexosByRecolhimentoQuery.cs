using MediatR;
using TrackCare.Application.DTOs;
using TrackCare.Domain.Interfaces;

namespace TrackCare.Application.Features.Anexos.Queries;

public record GetAnexosByRecolhimentoQuery(int RecolhimentoId) : IRequest<IEnumerable<AnexoDto>>;

public class GetAnexosByRecolhimentoQueryHandler : IRequestHandler<GetAnexosByRecolhimentoQuery, IEnumerable<AnexoDto>>
{
    private readonly IAnexoRepository _repository;
    private readonly AutoMapper.IMapper _mapper;

    public GetAnexosByRecolhimentoQueryHandler(IAnexoRepository repository, AutoMapper.IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AnexoDto>> Handle(GetAnexosByRecolhimentoQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetByRecolhimentoIdAsync(request.RecolhimentoId);
        return _mapper.Map<IEnumerable<AnexoDto>>(result);
    }
}
