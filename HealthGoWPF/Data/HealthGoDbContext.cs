using Microsoft.EntityFrameworkCore;
using HealthGoWPF.Models;

namespace HealthGoWPF.Data;

public class HealthGoDbContext : DbContext
{
    public DbSet<Recolhimento> Recolhimentos { get; set; } = null!;
    public DbSet<HistoricoStatus> HistoricoStatus { get; set; } = null!;
    public DbSet<Anexo> Anexos { get; set; } = null!;
    public DbSet<Comentario> Comentarios { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Conexão com Supabase PostgreSQL
        optionsBuilder.UseNpgsql("postgresql://postgres:ewcldebtsrjfdkckvqin@db.ewcldebtsrjfdkckvqin.supabase.co:5432/postgres");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Recolhimento>(entity =>
        {
            entity.HasIndex(e => e.Hgid);
            entity.HasIndex(e => e.NumeroSerie);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.DataSolicitacao);
            entity.HasIndex(e => e.ClienteNome);
        });

        modelBuilder.Entity<HistoricoStatus>(entity =>
        {
            entity.HasIndex(e => e.RecolhimentoId);
            entity.HasOne(e => e.Recolhimento)
                .WithMany()
                .HasForeignKey(e => e.RecolhimentoId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Anexo>(entity =>
        {
            entity.HasIndex(e => e.RecolhimentoId);
        });

        modelBuilder.Entity<Comentario>(entity =>
        {
            entity.HasIndex(e => e.RecolhimentoId);
            entity.HasOne(e => e.Recolhimento)
                .WithMany()
                .HasForeignKey(e => e.RecolhimentoId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
