using TrackCare.Domain.Entities;

namespace TrackCare.Domain.Interfaces;

public interface IAnexoRepository
{
    Task<IEnumerable<Anexo>> GetByRecolhimentoIdAsync(int recolhimentoId);
    Task<Anexo?> GetByIdAsync(int id);
    Task<Anexo> AddAsync(Anexo anexo);
    Task DeleteAsync(int id);
}
