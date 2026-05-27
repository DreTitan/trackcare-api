using TrackCare.Domain.Common;
using TrackCare.Domain.Enums;

namespace TrackCare.Domain.Entities;

public class Recolhimento : BaseAuditableEntity
{
    public string Hgid { get; set; } = string.Empty;
    public string NumeroSerie { get; set; } = string.Empty;
    public string? Modelo { get; set; }
    public string ClienteNome { get; set; } = string.Empty;
    public string? ClienteContato { get; set; }
    public string? ClienteEmail { get; set; }
    public string? ClienteTelefone { get; set; }
    public TipoPlano ClientePlano { get; set; } = TipoPlano.Simples;
    public string? TicketHub { get; set; }
    public string? TicketBlip { get; set; }
    public string? DescricaoProblema { get; set; }
    public string? RelatorioN3 { get; set; }
    public bool JaRecolhido { get; set; }
    public StatusRecolhimento Status { get; set; } = StatusRecolhimento.N3_Enviou;
    public DateTime DataSolicitacao { get; set; } = DateTime.UtcNow;
    public DateTime? DataPrevistaColeta { get; set; }
    public DateTime? DataColetaReal { get; set; }
    public DateTime? DataPrevistaDevolucao { get; set; }
    public DateTime? DataDevolucaoReal { get; set; }
    public string? Observacoes { get; set; }

    public ICollection<HistoricoStatus> HistoricoStatus { get; set; } = new List<HistoricoStatus>();
    public ICollection<Anexo> Anexos { get; set; } = new List<Anexo>();
    public ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();

    public string StatusTexto => Status.ToString().Replace("_", " ");
}
