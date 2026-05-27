using Microsoft.EntityFrameworkCore;
using TrackCare.Application.Common.Interfaces;
using TrackCare.Domain.Entities;

namespace TrackCare.Infrastructure.Persistence;

public class TrackCareDbContext : DbContext, IApplicationDbContext
{
    public TrackCareDbContext(DbContextOptions<TrackCareDbContext> options) : base(options)
    {
    }

    public DbSet<Recolhimento> Recolhimentos => Set<Recolhimento>();
    public DbSet<HistoricoStatus> HistoricoStatus => Set<HistoricoStatus>();
    public DbSet<Anexo> Anexos => Set<Anexo>();
    public DbSet<Comentario> Comentarios => Set<Comentario>();

    public new async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await base.SaveChangesAsync(cancellationToken);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Recolhimento>(entity =>
        {
            entity.ToTable("recolhimentos");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Hgid).IsRequired().HasMaxLength(50);
            entity.Property(e => e.NumeroSerie).IsRequired().HasMaxLength(100);
            entity.Property(e => e.ClienteNome).IsRequired().HasMaxLength(200);
            entity.Property(e => e.ClienteContato).HasMaxLength(200);
            entity.Property(e => e.ClienteEmail).HasMaxLength(200);
            entity.Property(e => e.ClienteTelefone).HasMaxLength(50);
            entity.Property(e => e.TicketHub).HasMaxLength(50);
            entity.Property(e => e.TicketBlip).HasMaxLength(50);
            entity.Property(e => e.DescricaoProblema).HasMaxLength(2000);
            entity.Property(e => e.RelatorioN3).HasMaxLength(4000);
            entity.Property(e => e.Observacoes).HasMaxLength(4000);
            entity.Property(e => e.CreatedBy).HasMaxLength(100);

            entity.HasIndex(e => e.Hgid);
            entity.HasIndex(e => e.NumeroSerie);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.DataSolicitacao);
            entity.HasIndex(e => e.ClienteNome);
        });

        modelBuilder.Entity<HistoricoStatus>(entity =>
        {
            entity.ToTable("historico_status");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Observacao).HasMaxLength(1000);
            entity.Property(e => e.Usuario).HasMaxLength(100);
            entity.HasOne(e => e.Recolhimento)
                .WithMany(r => r.HistoricoStatus)
                .HasForeignKey(e => e.RecolhimentoId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(e => e.RecolhimentoId);
        });

        modelBuilder.Entity<Anexo>(entity =>
        {
            entity.ToTable("anexos");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NomeOriginal).IsRequired().HasMaxLength(255);
            entity.Property(e => e.NomeArquivo).IsRequired().HasMaxLength(255);
            entity.Property(e => e.UsuarioUpload).HasMaxLength(100);
            entity.Property(e => e.CaminhoCompleto).HasMaxLength(500);
            entity.HasOne(e => e.Recolhimento)
                .WithMany(r => r.Anexos)
                .HasForeignKey(e => e.RecolhimentoId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(e => e.RecolhimentoId);
        });

        modelBuilder.Entity<Comentario>(entity =>
        {
            entity.ToTable("comentarios");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Texto).IsRequired().HasMaxLength(2000);
            entity.Property(e => e.Usuario).HasMaxLength(100);
            entity.Property(e => e.Setor).HasMaxLength(50);
            entity.HasOne(e => e.Recolhimento)
                .WithMany(r => r.Comentarios)
                .HasForeignKey(e => e.RecolhimentoId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(e => e.RecolhimentoId);
        });
    }
}
