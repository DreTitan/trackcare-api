using MediatR;
using TrackCare.Application.DTOs;
using TrackCare.Domain.Interfaces;

namespace TrackCare.Application.Features.Comentarios.Queries;

public record GetComentariosByRecolhimentoQuery(int RecolhimentoId) : IRequest<IEnumerable<ComentarioDto>>;

public class GetComentariosByRecolhimentoQueryHandler : IRequestHandler<GetComentariosByRecolhimentoQuery, IEnumerable<ComentarioDto>>
{
    private readonly IComentarioRepository _repository;
    private readonly AutoMapper.IMapper _mapper;

    public GetComentariosByRecolhimentoQueryHandler(IComentarioRepository repository, AutoMapper.IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ComentarioDto>> Handle(GetComentariosByRecolhimentoQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetByRecolhimentoIdAsync(request.RecolhimentoId);
        return _mapper.Map<IEnumerable<ComentarioDto>>(result);
    }
}
