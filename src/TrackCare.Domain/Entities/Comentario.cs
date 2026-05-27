using TrackCare.Domain.Common;

namespace TrackCare.Domain.Entities;

public class Comentario : BaseEntity
{
    public int RecolhimentoId { get; set; }
    public string Texto { get; set; } = string.Empty;
    public string? Usuario { get; set; }
    public string? Setor { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    public Recolhimento? Recolhimento { get; set; }
}
