using TrackCare.Domain.Enums;

namespace TrackCare.Domain.Entities;

public class HistoricoStatus
{
    public int Id { get; set; }
    public int RecolhimentoId { get; set; }
    public StatusRecolhimento StatusAnterior { get; set; }
    public StatusRecolhimento StatusNovo { get; set; }
    public string? Observacao { get; set; }
    public string? Usuario { get; set; }
    public DateTime DataAlteracao { get; set; } = DateTime.UtcNow;

    public Recolhimento? Recolhimento { get; set; }
}
