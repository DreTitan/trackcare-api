using TrackCare.Domain.Entities;
using TrackCare.Domain.Enums;

namespace TrackCare.Domain.Interfaces;

public interface IRecolhimentoRepository
{
    Task<Recolhimento?> GetByIdAsync(int id);
    Task<Recolhimento?> GetByIdWithDetailsAsync(int id);
    Task<IEnumerable<Recolhimento>> GetAllAsync();
    Task<IEnumerable<Recolhimento>> GetRecentAsync(int count);
    Task<IEnumerable<Recolhimento>> SearchAsync(string? termo, StatusRecolhimento? status);
    Task<Recolhimento> AddAsync(Recolhimento entity);
    Task UpdateAsync(Recolhimento entity);
    Task DeleteAsync(int id);
    Task AddStatusHistoryAsync(HistoricoStatus historico);
    Task<(int totalAtivos, int aguardaColeta, int emReparo, int encerradosMes)> GetStatsAsync();
}
