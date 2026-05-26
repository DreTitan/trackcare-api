using TrackCare.Domain.Entities;

namespace TrackCare.Domain.Interfaces;

public interface IComentarioRepository
{
    Task<IEnumerable<Comentario>> GetByRecolhimentoIdAsync(int recolhimentoId);
    Task<Comentario?> GetByIdAsync(int id);
    Task<Comentario> AddAsync(Comentario comentario);
    Task DeleteAsync(int id);
}
