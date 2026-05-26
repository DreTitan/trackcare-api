using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthGoCollection.Models;

public enum TipoAnexo
{
    RelatorioN3,
    PDF,
    Foto,
    NotaFiscal,
    Outro
}

public class Anexo
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int RecolhimentoId { get; set; }

    [ForeignKey("RecolhimentoId")]
    public Recolhimento? Recolhimento { get; set; }

    public TipoAnexo Tipo { get; set; } = TipoAnexo.Outro;

    [Required]
    [MaxLength(500)]
    public string CaminhoArquivo { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string NomeOriginal { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? ConteudoTipo { get; set; }

    public long TamanhoBytes { get; set; }

    [MaxLength(100)]
    public string? UsuarioUpload { get; set; }

    public DateTime DataUpload { get; set; } = DateTime.Now;
}
