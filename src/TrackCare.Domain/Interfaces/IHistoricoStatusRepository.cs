using TrackCare.Domain.Entities;

namespace TrackCare.Domain.Interfaces;

public interface IHistoricoStatusRepository
{
    Task<IEnumerable<HistoricoStatus>> GetByRecolhimentoIdAsync(int recolhimentoId);
    Task AddAsync(HistoricoStatus historico);
}
