using HealthGoCollection.Models;

namespace HealthGoCollection.Services;

public interface IRecolhimentoService
{
    Task<List<Recolhimento>> GetAllAsync();
    Task<Recolhimento?> GetByIdAsync(int id);
    Task<Recolhimento?> GetByIdWithDetailsAsync(int id);
    Task<List<Recolhimento>> SearchAsync(string? termo, StatusRecolhimento? status);
    Task<Recolhimento> CreateAsync(Recolhimento recolhimento, string usuario);
    Task<Recolhimento> UpdateAsync(Recolhimento recolhimento, string usuario);
    Task<bool> UpdateStatusAsync(int id, StatusRecolhimento novoStatus, string? observacao, string usuario);
    Task<bool> DeleteAsync(int id);
    Task<DashboardStats> GetDashboardStatsAsync();
}

public class DashboardStats
{
    public int TotalAtivos { get; set; }
    public int AguardaColeta { get; set; }
    public int EmReparo { get; set; }
    public int EncerradosMes { get; set; }
    public double TempoMedioDias { get; set; }
    public double TaxaSLA { get; set; }
}
