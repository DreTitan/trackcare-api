using Microsoft.EntityFrameworkCore;
using HealthGoCollection.Models;

namespace HealthGoCollection.Data;

public class HealthGoDbContext : DbContext
{
    public HealthGoDbContext(DbContextOptions<HealthGoDbContext> options)
        : base(options)
    {
    }

    public DbSet<Recolhimento> Recolhimentos { get; set; } = null!;
    public DbSet<Anexo> Anexos { get; set; } = null!;
    public DbSet<HistoricoStatus> HistoricoStatus { get; set; } = null!;
    public DbSet<Usuario> Usuarios { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Recolhimento
        modelBuilder.Entity<Recolhimento>(entity =>
        {
            entity.HasIndex(e => e.Hgid);
            entity.HasIndex(e => e.NumeroSerie);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.DataSolicitacao);
            entity.HasIndex(e => e.ClienteNome);

            entity.Property(e => e.Status)
                .HasConversion<string>()
                .HasMaxLength(50);

            entity.Property(e => e.ClientePlano)
                .HasConversion<string>()
                .HasMaxLength(20);
        });

        // Anexo
        modelBuilder.Entity<Anexo>(entity =>
        {
            entity.HasIndex(e => e.RecolhimentoId);
            entity.HasOne(e => e.Recolhimento)
                .WithMany(r => r.Anexos)
                .HasForeignKey(e => e.RecolhimentoId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(e => e.Tipo)
                .HasConversion<string>()
                .HasMaxLength(50);
        });

        // HistoricoStatus
        modelBuilder.Entity<HistoricoStatus>(entity =>
        {
            entity.HasIndex(e => e.RecolhimentoId);
            entity.HasOne(e => e.Recolhimento)
                .WithMany(r => r.HistoricoStatus)
                .HasForeignKey(e => e.RecolhimentoId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(e => e.StatusAnterior)
                .HasConversion<string>()
                .HasMaxLength(50);

            entity.Property(e => e.StatusNovo)
                .HasConversion<string>()
                .HasMaxLength(50);
        });

        // Usuario
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasIndex(e => e.Login).IsUnique();

            entity.Property(e => e.Role)
                .HasConversion<string>()
                .HasMaxLength(20);

            // Seed admin user (senha: admin123) - hash simples para demo
            entity.HasData(new Usuario
            {
                Id = 1,
                Nome = "Administrador",
                Login = "admin",
                SenhaHash = "admin123", // Em produção usar hash real
                Role = RoleUsuario.Admin,
                Ativo = true
            });
        });
    }
}
