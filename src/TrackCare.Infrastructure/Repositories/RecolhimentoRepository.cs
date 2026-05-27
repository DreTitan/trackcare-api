using Microsoft.EntityFrameworkCore;
using TrackCare.Domain.Entities;
using TrackCare.Domain.Enums;
using TrackCare.Domain.Interfaces;
using TrackCare.Infrastructure.Persistence;

namespace TrackCare.Infrastructure.Repositories;

public class RecolhimentoRepository : IRecolhimentoRepository
{
    private readonly TrackCareDbContext _context;

    public RecolhimentoRepository(TrackCareDbContext context)
    {
        _context = context;
    }

    public async Task<Recolhimento?> GetByIdAsync(int id)
        => await _context.Recolhimentos.FindAsync(id);

    public async Task<Recolhimento?> GetByIdWithDetailsAsync(int id)
        => await _context.Recolhimentos
            .Include(r => r.HistoricoStatus)
            .Include(r => r.Anexos)
            .Include(r => r.Comentarios)
            .FirstOrDefaultAsync(r => r.Id == id);

    public async Task<IEnumerable<Recolhimento>> GetAllAsync()
        => await _context.Recolhimentos
            .OrderByDescending(r => r.DataSolicitacao)
            .ToListAsync();

    public async Task<IEnumerable<Recolhimento>> GetRecentAsync(int count)
        => await _context.Recolhimentos
            .OrderByDescending(r => r.DataSolicitacao)
            .Take(count)
            .ToListAsync();

    public async Task<IEnumerable<Recolhimento>> SearchAsync(string? termo, StatusRecolhimento? status)
    {
        var query = _context.Recolhimentos.AsQueryable();

        if (!string.IsNullOrWhiteSpace(termo))
        {
            termo = termo.ToLower();
            query = query.Where(r =>
                r.Hgid.ToLower().Contains(termo) ||
                r.NumeroSerie.ToLower().Contains(termo) ||
                r.ClienteNome.ToLower().Contains(termo) ||
                (r.TicketHub != null && r.TicketHub.ToLower().Contains(termo)) ||
                (r.TicketBlip != null && r.TicketBlip.ToLower().Contains(termo)));
        }

        if (status.HasValue)
            query = query.Where(r => r.Status == status.Value);

        return await query.OrderByDescending(r => r.DataSolicitacao).ToListAsync();
    }

    public async Task<Recolhimento> AddAsync(Recolhimento entity)
    {
        _context.Recolhimentos.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(Recolhimento entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Recolhimentos.FindAsync(id);
        if (entity != null)
        {
            _context.Recolhimentos.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task AddStatusHistoryAsync(HistoricoStatus historico)
    {
        _context.HistoricoStatus.Add(historico);
        await _context.SaveChangesAsync();
    }

    public async Task<(int totalAtivos, int aguardaColeta, int emReparo, int encerradosMes)> GetStatsAsync()
    {
        var totalAtivos = await _context.Recolhimentos
            .Where(r => r.Status != StatusRecolhimento.Encerrado)
            .CountAsync();

        var aguardaColeta = await _context.Recolhimentos
            .Where(r => r.Status == StatusRecolhimento.Aguarda_Coleta ||
                       r.Status == StatusRecolhimento.Aguarda_Admin ||
                       r.Status == StatusRecolhimento.N3_Enviou)
            .CountAsync();

        var emReparo = await _context.Recolhimentos
            .Where(r => r.Status == StatusRecolhimento.Em_Reparo ||
                       r.Status == StatusRecolhimento.Recebido_Fabrica ||
                       r.Status == StatusRecolhimento.Em_Transito)
            .CountAsync();

        var primeiroDiaMes = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
        var encerradosMes = await _context.Recolhimentos
            .Where(r => r.Status == StatusRecolhimento.Encerrado &&
                       r.LastModified >= primeiroDiaMes)
            .CountAsync();

        return (totalAtivos, aguardaColeta, emReparo, encerradosMes);
    }
}
