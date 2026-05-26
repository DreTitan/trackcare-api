using MediatR;
using TrackCare.Application.DTOs;
using TrackCare.Domain.Interfaces;

namespace TrackCare.Application.Features.Recolhimentos.Queries;

public record GetRecolhimentoByIdQuery(int Id) : IRequest<RecolhimentoDto?>;

public class GetRecolhimentoByIdQueryHandler : IRequestHandler<GetRecolhimentoByIdQuery, RecolhimentoDto?>
{
    private readonly IRecolhimentoRepository _repository;
    private readonly AutoMapper.IMapper _mapper;

    public GetRecolhimentoByIdQueryHandler(IRecolhimentoRepository repository, AutoMapper.IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<RecolhimentoDto?> Handle(GetRecolhimentoByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetByIdAsync(request.Id);
        return result == null ? null : _mapper.Map<RecolhimentoDto>(result);
    }
}
