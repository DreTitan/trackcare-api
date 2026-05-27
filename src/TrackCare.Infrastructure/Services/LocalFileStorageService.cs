using Microsoft.Extensions.Options;
using TrackCare.Domain.Interfaces;
using TrackCare.Infrastructure.Configuration;

namespace TrackCare.Infrastructure.Services;

public class LocalFileStorageService : IFileStorageService
{
    private readonly string _basePath;

    public LocalFileStorageService(SupabaseSettings settings)
    {
        _basePath = settings.Bucket; // usará como path base
        Directory.CreateDirectory(_basePath);
    }

    public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType)
    {
        var nomeUnico = $"{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid():N}_{fileName}";
        var caminhoCompleto = Path.Combine(_basePath, nomeUnico);

        await using var fs = new FileStream(caminhoCompleto, FileMode.Create);
        await fileStream.CopyToAsync(fs);

        // Retorna caminho acessível via API
        return $"/files/{nomeUnico}";
    }

    public Task<string> GetPublicUrlAsync(string fileName)
    {
        return Task.FromResult($"/files/{fileName}");
    }

    public Task DeleteAsync(string fileName)
    {
        var caminho = Path.Combine(_basePath, fileName);
        if (File.Exists(caminho))
            File.Delete(caminho);
        return Task.CompletedTask;
    }
}
