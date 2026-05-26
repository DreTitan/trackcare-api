using TrackCare.Domain.Enums;

namespace TrackCare.Domain.Entities;

public class Anexo
{
    public int Id { get; set; }
    public int RecolhimentoId { get; set; }
    public TipoAnexo Tipo { get; set; } = TipoAnexo.PDF;
    public string NomeOriginal { get; set; } = string.Empty;
    public string NomeArquivo { get; set; } = string.Empty;
    public long TamanhoBytes { get; set; }
    public string? UsuarioUpload { get; set; }
    public DateTime DataUpload { get; set; } = DateTime.UtcNow;
    public string CaminhoCompleto { get; set; } = string.Empty;

    public Recolhimento? Recolhimento { get; set; }
}
