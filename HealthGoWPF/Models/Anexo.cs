using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthGoWPF.Models;

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

    public TipoAnexo Tipo { get; set; } = TipoAnexo.PDF;

    [Required]
    [MaxLength(255)]
    public string NomeOriginal { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string NomeArquivo { get; set; } = string.Empty;

    public long TamanhoBytes { get; set; }

    [MaxLength(100)]
    public string? UsuarioUpload { get; set; }

    public DateTime DataUpload { get; set; } = DateTime.Now;

    public string CaminhoCompleto { get; set; } = string.Empty;

    // Propriedade não persistida para exibição
    [NotMapped]
    public string TamanhoFormatado { get; set; } = string.Empty;
}
