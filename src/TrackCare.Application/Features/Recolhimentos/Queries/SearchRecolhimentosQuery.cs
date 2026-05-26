using MediatR;
using TrackCare.Application.DTOs;
using TrackCare.Domain.Enums;
using TrackCare.Domain.Interfaces;

namespace TrackCare.Application.Features.Recolhimentos.Queries;

public record SearchRecolhimentosQuery(string? Termo, string? Status) : IRequest<IEnumerable<RecolhimentoDto>>;

public class SearchRecolhimentosQueryHandler : IRequestHandler<SearchRecolhimentosQuery, IEnumerable<RecolhimentoDto>>
{
    private readonly IRecolhimentoRepository _repository;
    private readonly AutoMapper.IMapper _mapper;

    public SearchRecolhimentosQueryHandler(IRecolhimentoRepository repository, AutoMapper.IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RecolhimentoDto>> Handle(SearchRecolhimentosQuery request, CancellationToken cancellationToken)
    {
        StatusRecolhimento? status = null;
        if (!string.IsNullOrEmpty(request.Status))
            status = Enum.Parse<StatusRecolhimento>(request.Status);

        var result = await _repository.SearchAsync(request.Termo, status);
        return _mapper.Map<IEnumerable<RecolhimentoDto>>(result);
    }
}
