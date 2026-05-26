using Supabase;
using TrackCare.Domain.Interfaces;
using TrackCare.Infrastructure.Configuration;

namespace TrackCare.Infrastructure.Services;

public class SupabaseStorageService : IFileStorageService
{
    private readonly Client _client;
    private readonly string _bucket;

    public SupabaseStorageService(SupabaseSettings settings)
    {
        _bucket = settings.Bucket;
        var options = new SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = false
        };
        _client = new Client(settings.Url, settings.Key, options);
    }

    public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType)
    {
        var nomeUnico = $"{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid():N}_{fileName}";

        using var ms = new MemoryStream();
        await fileStream.CopyToAsync(ms);
        var bytes = ms.ToArray();

        var bucket = _client.Storage.From(_bucket);
        await bucket.Upload(bytes, nomeUnico);
        return bucket.GetPublicUrl(nomeUnico);
    }

    public Task<string> GetPublicUrlAsync(string fileName)
    {
        var bucket = _client.Storage.From(_bucket);
        return Task.FromResult(bucket.GetPublicUrl(fileName));
    }

    public async Task DeleteAsync(string fileName)
    {
        var bucket = _client.Storage.From(_bucket);
        await bucket.Remove(fileName);
    }
}
