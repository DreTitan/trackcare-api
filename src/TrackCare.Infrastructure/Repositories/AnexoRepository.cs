using Microsoft.EntityFrameworkCore;
using TrackCare.Domain.Entities;
using TrackCare.Domain.Interfaces;
using TrackCare.Infrastructure.Persistence;

namespace TrackCare.Infrastructure.Repositories;

public class AnexoRepository : IAnexoRepository
{
    private readonly TrackCareDbContext _context;

    public AnexoRepository(TrackCareDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Anexo>> GetByRecolhimentoIdAsync(int recolhimentoId)
        => await _context.Anexos
            .Where(a => a.RecolhimentoId == recolhimentoId)
            .OrderByDescending(a => a.DataUpload)
            .ToListAsync();

    public async Task<Anexo?> GetByIdAsync(int id)
        => await _context.Anexos.FindAsync(id);

    public async Task<Anexo> AddAsync(Anexo anexo)
    {
        _context.Anexos.Add(anexo);
        await _context.SaveChangesAsync();
        return anexo;
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Anexos.FindAsync(id);
        if (entity != null)
        {
            _context.Anexos.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
