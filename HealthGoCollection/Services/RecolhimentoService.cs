using Microsoft.EntityFrameworkCore;
using HealthGoCollection.Data;
using HealthGoCollection.Models;

namespace HealthGoCollection.Services;

public class RecolhimentoService : IRecolhimentoService
{
    private readonly HealthGoDbContext _context;

    public RecolhimentoService(HealthGoDbContext context)
    {
        _context = context;
    }

    public async Task<List<Recolhimento>> GetAllAsync()
    {
        return await _context.Recolhimentos
            .Include(r => r.Anexos)
            .OrderByDescending(r => r.DataSolicitacao)
            .ToListAsync();
    }

    public async Task<Recolhimento?> GetByIdAsync(int id)
    {
        return await _context.Recolhimentos.FindAsync(id);
    }

    public async Task<Recolhimento?> GetByIdWithDetailsAsync(int id)
    {
        return await _context.Recolhimentos
            .Include(r => r.Anexos)
            .Include(r => r.HistoricoStatus.OrderByDescending(h => h.DataAlteracao))
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<List<Recolhimento>> SearchAsync(string? termo, StatusRecolhimento? status)
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
                (r.TicketBlip != null && r.TicketBlip.ToLower().Contains(termo)) ||
                (r.DescricaoProblema != null && r.DescricaoProblema.ToLower().Contains(termo)));
        }

        if (status.HasValue)
        {
            query = query.Where(r => r.Status == status.Value);
        }

        return await query
            .Include(r => r.Anexos)
            .OrderByDescending(r => r.DataSolicitacao)
            .ToListAsync();
    }

    public async Task<Recolhimento> CreateAsync(Recolhimento recolhimento, string usuario)
    {
        recolhimento.CriadoPor = usuario;
        recolhimento.CriadoEm = DateTime.Now;
        recolhimento.AtualizadoEm = DateTime.Now;

        _context.Recolhimentos.Add(recolhimento);
        await _context.SaveChangesAsync();

        // Registrar no histórico
        var historico = new HistoricoStatus
        {
            RecolhimentoId = recolhimento.Id,
            StatusAnterior = (StatusRecolhimento)(-1),
            StatusNovo = recolhimento.Status,
            Observacao = "Recolhimento criado",
            Usuario = usuario
        };
        _context.HistoricoStatus.Add(historico);
        await _context.SaveChangesAsync();

        return recolhimento;
    }

    public async Task<Recolhimento> UpdateAsync(Recolhimento recolhimento, string usuario)
    {
        var existente = await _context.Recolhimentos.FindAsync(recolhimento.Id);
        if (existente == null)
            throw new Exception("Recolhimento não encontrado");

        existente.Hgid = recolhimento.Hgid;
        existente.NumeroSerie = recolhimento.NumeroSerie;
        existente.Modelo = recolhimento.Modelo;
        existente.ClienteNome = recolhimento.ClienteNome;
        existente.ClienteContato = recolhimento.ClienteContato;
        existente.ClienteEmail = recolhimento.ClienteEmail;
        existente.ClienteTelefone = recolhimento.ClienteTelefone;
        existente.ClientePlano = recolhimento.ClientePlano;
        existente.TicketHub = recolhimento.TicketHub;
        existente.TicketBlip = recolhimento.TicketBlip;
        existente.DescricaoProblema = recolhimento.DescricaoProblema;
        existente.RelatorioN3 = recolhimento.RelatorioN3;
        existente.JaRecolhido = recolhimento.JaRecolhido;
        existente.DataPrevistaColeta = recolhimento.DataPrevistaColeta;
        existente.DataColetaReal = recolhimento.DataColetaReal;
        existente.DataPrevistaDevolucao = recolhimento.DataPrevistaDevolucao;
        existente.DataDevolucaoReal = recolhimento.DataDevolucaoReal;
        existente.Observacoes = recolhimento.Observacoes;
        existente.AtualizadoEm = DateTime.Now;

        await _context.SaveChangesAsync();
        return existente;
    }

    public async Task<bool> UpdateStatusAsync(int id, StatusRecolhimento novoStatus, string? observacao, string usuario)
    {
        var recolhimento = await _context.Recolhimentos.FindAsync(id);
        if (recolhimento == null)
            return false;

        var statusAnterior = recolhimento.Status;
        recolhimento.Status = novoStatus;
        recolhimento.AtualizadoEm = DateTime.Now;

        // Registrar no histórico
        var historico = new HistoricoStatus
        {
            RecolhimentoId = id,
            StatusAnterior = statusAnterior,
            StatusNovo = novoStatus,
            Observacao = observacao,
            Usuario = usuario
        };
        _context.HistoricoStatus.Add(historico);

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var recolhimento = await _context.Recolhimentos.FindAsync(id);
        if (recolhimento == null)
            return false;

        _context.Recolhimentos.Remove(recolhimento);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<DashboardStats> GetDashboardStatsAsync()
    {
        var total = await _context.Recolhimentos.CountAsync();
        var ativos = await _context.Recolhimentos
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

        var primeiroDiaMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var encerradosMes = await _context.Recolhimentos
            .Where(r => r.Status == StatusRecolhimento.Encerrado &&
                       r.AtualizadoEm >= primeiroDiaMes)
            .CountAsync();

        // Calcular tempo médio dos encerrados no último mês
        var encerradosRecentes = await _context.Recolhimentos
            .Where(r => r.Status == StatusRecolhimento.Encerrado &&
                       r.AtualizadoEm >= primeiroDiaMes)
            .ToListAsync();

        var tempoMedio = encerradosRecentes.Any()
            ? encerradosRecentes.Average(r => (r.AtualizadoEm - r.DataSolicitacao).TotalDays)
            : 0;

        return new DashboardStats
        {
            TotalAtivos = ativos,
            AguardaColeta = aguardaColeta,
            EmReparo = emReparo,
            EncerradosMes = encerradosMes,
            TempoMedioDias = Math.Round(tempoMedio, 1),
            TaxaSLA = total > 0 ? Math.Round((double)encerradosMes / total * 100, 1) : 100
        };
    }
}
