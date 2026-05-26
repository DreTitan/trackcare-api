using MediatR;
using TrackCare.Application.DTOs;
using TrackCare.Domain.Interfaces;

namespace TrackCare.Application.Features.Recolhimentos.Queries;

public record GetDashboardStatsQuery : IRequest<DashboardStatsDto>;

public class GetDashboardStatsQueryHandler : IRequestHandler<GetDashboardStatsQuery, DashboardStatsDto>
{
    private readonly IRecolhimentoRepository _repository;

    public GetDashboardStatsQueryHandler(IRecolhimentoRepository repository)
    {
        _repository = repository;
    }

    public async Task<DashboardStatsDto> Handle(GetDashboardStatsQuery request, CancellationToken cancellationToken)
    {
        var (totalAtivos, aguardaColeta, emReparo, encerradosMes) = await _repository.GetStatsAsync();
        return new DashboardStatsDto(totalAtivos, aguardaColeta, emReparo, encerradosMes);
    }
}
