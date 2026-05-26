using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthGoCollection.Models;

public enum StatusRecolhimento
{
    N3_Enviou,
    Aguarda_Admin,
    Aguarda_Coleta,
    Em_Transito,
    Recebido_Fabrica,
    Em_Reparo,
    Calibracao,
    Pronto_Devolver,
    Enviado,
    Follow_Up,
    Encerrado
}

public enum TipoPlano
{
    GoPremium,
    Simples
}

public class Recolhimento
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Hgid { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string NumeroSerie { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Modelo { get; set; }

    [Required]
    [MaxLength(200)]
    public string ClienteNome { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? ClienteContato { get; set; }

    [MaxLength(200)]
    public string? ClienteEmail { get; set; }

    [MaxLength(50)]
    public string? ClienteTelefone { get; set; }

    public TipoPlano ClientePlano { get; set; } = TipoPlano.Simples;

    [MaxLength(50)]
    public string? TicketHub { get; set; }

    [MaxLength(50)]
    public string? TicketBlip { get; set; }

    [MaxLength(2000)]
    public string? DescricaoProblema { get; set; }

    [MaxLength(4000)]
    public string? RelatorioN3 { get; set; }

    public bool JaRecolhido { get; set; } = false;

    public StatusRecolhimento Status { get; set; } = StatusRecolhimento.N3_Enviou;

    public DateTime DataSolicitacao { get; set; } = DateTime.Now;

    public DateTime? DataPrevistaColeta { get; set; }

    public DateTime? DataColetaReal { get; set; }

    public DateTime? DataPrevistaDevolucao { get; set; }

    public DateTime? DataDevolucaoReal { get; set; }

    [MaxLength(4000)]
    public string? Observacoes { get; set; }

    [MaxLength(100)]
    public string? CriadoPor { get; set; }

    public DateTime CriadoEm { get; set; } = DateTime.Now;

    public DateTime AtualizadoEm { get; set; } = DateTime.Now;

    // Navigation properties
    public ICollection<Anexo> Anexos { get; set; } = new List<Anexo>();
    public ICollection<HistoricoStatus> HistoricoStatus { get; set; } = new List<HistoricoStatus>();
}
