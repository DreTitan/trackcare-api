using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthGoWPF.Models;

public class HistoricoStatus
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int RecolhimentoId { get; set; }

    [ForeignKey("RecolhimentoId")]
    public Recolhimento? Recolhimento { get; set; }

    public StatusRecolhimento StatusAnterior { get; set; }

    public StatusRecolhimento StatusNovo { get; set; }

    [MaxLength(1000)]
    public string? Observacao { get; set; }

    [MaxLength(100)]
    public string? Usuario { get; set; }

    public DateTime DataAlteracao { get; set; } = DateTime.Now;
}
