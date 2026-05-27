using Xunit;
using Microsoft.EntityFrameworkCore;
using TrackCare.Domain.Entities;
using TrackCare.Domain.Enums;
using TrackCare.Infrastructure.Persistence;
using TrackCare.Infrastructure.Repositories;

namespace TrackCare.Infrastructure.Tests.Repositories;

public class RecolhimentoRepositoryTests : IDisposable
{
    private readonly TrackCareDbContext _context;
    private readonly RecolhimentoRepository _repository;

    public RecolhimentoRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<TrackCareDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TrackCareDbContext(options);
        _repository = new RecolhimentoRepository(_context);
    }

    private async Task<Recolhimento> SeedAsync(Recolhimento r)
    {
        _context.Recolhimentos.Add(r);
        await _context.SaveChangesAsync();
        return r;
    }

    [Fact]
    public async Task AddAsync_should_add_and_return_entity()
    {
        var entity = new Recolhimento
        {
            Hgid = "HG001",
            NumeroSerie = "SN001",
            ClienteNome = "Teste",
            Status = StatusRecolhimento.N3_Enviou,
            DataSolicitacao = DateTime.UtcNow,
            Created = DateTimeOffset.UtcNow,
            LastModified = DateTimeOffset.UtcNow
        };

        var result = await _repository.AddAsync(entity);

        Assert.True(result.Id > 0);
        Assert.Equal("HG001", result.Hgid);
    }

    [Fact]
    public async Task GetByIdAsync_should_return_entity()
    {
        var seeded = await SeedAsync(new Recolhimento
        {
            Hgid = "HG002",
            NumeroSerie = "SN002",
            ClienteNome = "Cliente 2",
            Status = StatusRecolhimento.Aguarda_Coleta,
            DataSolicitacao = DateTime.UtcNow,
            Created = DateTimeOffset.UtcNow,
            LastModified = DateTimeOffset.UtcNow
        });

        var result = await _repository.GetByIdAsync(seeded.Id);

        Assert.NotNull(result);
        Assert.Equal("HG002", result!.Hgid);
    }

    [Fact]
    public async Task GetByIdAsync_should_return_null_for_nonexistent()
    {
        var result = await _repository.GetByIdAsync(9999);
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_should_return_ordered_by_DataSolicitacao_desc()
    {
        await SeedAsync(new Recolhimento { Hgid = "HG-A", NumeroSerie = "S1", ClienteNome = "A", Status = StatusRecolhimento.N3_Enviou, DataSolicitacao = DateTime.UtcNow.AddDays(-2), Created = DateTimeOffset.UtcNow, LastModified = DateTimeOffset.UtcNow });
        await SeedAsync(new Recolhimento { Hgid = "HG-B", NumeroSerie = "S2", ClienteNome = "B", Status = StatusRecolhimento.N3_Enviou, DataSolicitacao = DateTime.UtcNow, Created = DateTimeOffset.UtcNow, LastModified = DateTimeOffset.UtcNow });

        var result = (await _repository.GetAllAsync()).ToList();

        Assert.Equal(2, result.Count);
        Assert.Equal("HG-B", result[0].Hgid);
        Assert.Equal("HG-A", result[1].Hgid);
    }

    [Fact]
    public async Task SearchAsync_should_filter_by_termo()
    {
        await SeedAsync(new Recolhimento { Hgid = "HG-FIND", NumeroSerie = "SN-FIND", ClienteNome = "Busca Cliente", Status = StatusRecolhimento.N3_Enviou, DataSolicitacao = DateTime.UtcNow, Created = DateTimeOffset.UtcNow, LastModified = DateTimeOffset.UtcNow });
        await SeedAsync(new Recolhimento { Hgid = "HG-OTHER", NumeroSerie = "SN-OTHER", ClienteNome = "Outro Cliente", Status = StatusRecolhimento.N3_Enviou, DataSolicitacao = DateTime.UtcNow, Created = DateTimeOffset.UtcNow, LastModified = DateTimeOffset.UtcNow });

        var result = (await _repository.SearchAsync("find", null)).ToList();

        Assert.Single(result);
        Assert.Equal("HG-FIND", result[0].Hgid);
    }

    [Fact]
    public async Task SearchAsync_should_filter_by_status()
    {
        await SeedAsync(new Recolhimento { Hgid = "HG-ACTIVE", NumeroSerie = "S1", ClienteNome = "C1", Status = StatusRecolhimento.N3_Enviou, DataSolicitacao = DateTime.UtcNow, Created = DateTimeOffset.UtcNow, LastModified = DateTimeOffset.UtcNow });
        await SeedAsync(new Recolhimento { Hgid = "HG-CLOSED", NumeroSerie = "S2", ClienteNome = "C2", Status = StatusRecolhimento.Encerrado, DataSolicitacao = DateTime.UtcNow, Created = DateTimeOffset.UtcNow, LastModified = DateTimeOffset.UtcNow });

        var result = (await _repository.SearchAsync(null, StatusRecolhimento.N3_Enviou)).ToList();

        Assert.Single(result);
        Assert.Equal("HG-ACTIVE", result[0].Hgid);
    }

    [Fact]
    public async Task GetStatsAsync_should_return_correct_counts()
    {
        await SeedAsync(new Recolhimento { Hgid = "HG-A", NumeroSerie = "S1", ClienteNome = "C1", Status = StatusRecolhimento.N3_Enviou, DataSolicitacao = DateTime.UtcNow, Created = DateTimeOffset.UtcNow, LastModified = DateTimeOffset.UtcNow });
        await SeedAsync(new Recolhimento { Hgid = "HG-B", NumeroSerie = "S2", ClienteNome = "C2", Status = StatusRecolhimento.Em_Reparo, DataSolicitacao = DateTime.UtcNow, Created = DateTimeOffset.UtcNow, LastModified = DateTimeOffset.UtcNow });
        await SeedAsync(new Recolhimento { Hgid = "HG-C", NumeroSerie = "S3", ClienteNome = "C3", Status = StatusRecolhimento.Encerrado, DataSolicitacao = DateTime.UtcNow, Created = DateTimeOffset.UtcNow, LastModified = DateTimeOffset.UtcNow });

        var (totalAtivos, aguardaColeta, emReparo, encerradosMes) = await _repository.GetStatsAsync();

        Assert.Equal(2, totalAtivos);
        Assert.Equal(1, aguardaColeta);
        Assert.Equal(1, emReparo);
        Assert.Equal(1, encerradosMes);
    }

    [Fact]
    public async Task DeleteAsync_should_remove_entity()
    {
        var seeded = await SeedAsync(new Recolhimento { Hgid = "HG-DEL", NumeroSerie = "S1", ClienteNome = "C1", Status = StatusRecolhimento.N3_Enviou, DataSolicitacao = DateTime.UtcNow, Created = DateTimeOffset.UtcNow, LastModified = DateTimeOffset.UtcNow });

        await _repository.DeleteAsync(seeded.Id);
        var result = await _repository.GetByIdAsync(seeded.Id);

        Assert.Null(result);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
