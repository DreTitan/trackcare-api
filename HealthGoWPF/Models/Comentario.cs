using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthGoWPF.Models;

public class Comentario
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int RecolhimentoId { get; set; }

    [ForeignKey("RecolhimentoId")]
    public Recolhimento? Recolhimento { get; set; }

    [Required]
    [MaxLength(2000)]
    public string Texto { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Usuario { get; set; }

    [MaxLength(50)]
    public string? Setor { get; set; }

    public DateTime DataCriacao { get; set; } = DateTime.Now;
}
