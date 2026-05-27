using Microsoft.EntityFrameworkCore;
using TrackCare.Domain.Entities;

namespace TrackCare.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Recolhimento> Recolhimentos { get; }
    DbSet<HistoricoStatus> HistoricoStatus { get; }
    DbSet<Anexo> Anexos { get; }
    DbSet<Comentario> Comentarios { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
