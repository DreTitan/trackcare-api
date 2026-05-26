using Microsoft.EntityFrameworkCore;
using TrackCare.Domain.Entities;
using TrackCare.Domain.Interfaces;
using TrackCare.Infrastructure.Persistence;

namespace TrackCare.Infrastructure.Repositories;

public class HistoricoStatusRepository : IHistoricoStatusRepository
{
    private readonly TrackCareDbContext _context;

    public HistoricoStatusRepository(TrackCareDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<HistoricoStatus>> GetByRecolhimentoIdAsync(int recolhimentoId)
        => await _context.HistoricoStatus
            .Where(h => h.RecolhimentoId == recolhimentoId)
            .OrderByDescending(h => h.DataAlteracao)
            .ToListAsync();

    public async Task AddAsync(HistoricoStatus historico)
    {
        _context.HistoricoStatus.Add(historico);
        await _context.SaveChangesAsync();
    }
}
