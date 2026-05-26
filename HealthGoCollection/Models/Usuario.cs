using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthGoCollection.Models;

public enum RoleUsuario
{
    Admin,
    N1,
    N2,
    N3,
    Operador
}

public class Usuario
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Login { get; set; } = string.Empty;

    [Required]
    public string SenhaHash { get; set; } = string.Empty;

    public RoleUsuario Role { get; set; } = RoleUsuario.Operador;

    public bool Ativo { get; set; } = true;

    public DateTime? UltimoAcesso { get; set; }

    public DateTime CriadoEm { get; set; } = DateTime.Now;
}
