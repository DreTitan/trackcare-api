using Microsoft.EntityFrameworkCore;
using TrackCare.Domain.Entities;
using TrackCare.Domain.Interfaces;
using TrackCare.Infrastructure.Persistence;

namespace TrackCare.Infrastructure.Repositories;

public class ComentarioRepository : IComentarioRepository
{
    private readonly TrackCareDbContext _context;

    public ComentarioRepository(TrackCareDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Comentario>> GetByRecolhimentoIdAsync(int recolhimentoId)
        => await _context.Comentarios
            .Where(c => c.RecolhimentoId == recolhimentoId)
            .OrderByDescending(c => c.DataCriacao)
            .ToListAsync();

    public async Task<Comentario?> GetByIdAsync(int id)
        => await _context.Comentarios.FindAsync(id);

    public async Task<Comentario> AddAsync(Comentario comentario)
    {
        _context.Comentarios.Add(comentario);
        await _context.SaveChangesAsync();
        return comentario;
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Comentarios.FindAsync(id);
        if (entity != null)
        {
            _context.Comentarios.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
