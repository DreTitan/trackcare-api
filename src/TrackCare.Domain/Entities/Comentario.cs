namespace TrackCare.Domain.Entities;

public class Comentario
{
    public int Id { get; set; }
    public int RecolhimentoId { get; set; }
    public string Texto { get; set; } = string.Empty;
    public string? Usuario { get; set; }
    public string? Setor { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    public Recolhimento? Recolhimento { get; set; }
}
